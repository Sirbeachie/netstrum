#include <winsock2.h>
#include <windows.h>
#include <detours.h>
#include <fmt/core.h>
#include <string>
#include <vector>
#include <optional>

#pragma comment(lib, "ws2_32.lib")

class TcpClient {
public:
    TcpClient(const std::string& host, uint16_t port) {
        WSADATA wsa{};
        WSAStartup(MAKEWORD(2,2), &wsa);
        sockaddr_in addr{};
        addr.sin_family = AF_INET;
        addr.sin_port = htons(port);
        addr.sin_addr.s_addr = inet_addr(host.c_str());
        sock_ = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
        if (connect(sock_, reinterpret_cast<sockaddr*>(&addr), sizeof(addr)) == SOCKET_ERROR) {
            fmt::print("TcpClient: failed to connect\n");
            closesocket(sock_);
            sock_ = INVALID_SOCKET;
        }
    }

    ~TcpClient() {
        if (sock_ != INVALID_SOCKET) {
            closesocket(sock_);
        }
        WSACleanup();
    }

    void sendPacket(const char* data, int len) {
        if (sock_ == INVALID_SOCKET) return;
        send(sock_, data, len, 0);
    }

    std::optional<std::string> receiveCommand() {
        if (sock_ == INVALID_SOCKET) return std::nullopt;
        u_long bytes = 0;
        ioctlsocket(sock_, FIONREAD, &bytes);
        if (bytes == 0) return std::nullopt;
        std::vector<char> buf(bytes);
        int ret = recv(sock_, buf.data(), static_cast<int>(buf.size()), 0);
        if (ret <= 0) return std::nullopt;
        return std::string(buf.begin(), buf.begin() + ret);
    }

private:
    SOCKET sock_ = INVALID_SOCKET;
};

static TcpClient g_client("127.0.0.1", 7331);

static decltype(&send) real_send = send;
static decltype(&recv) real_recv = recv;

int WSAAPI HookedSend(SOCKET s, const char* buf, int len, int flags) {
    fmt::print("send intercepted: {} bytes\n", len);
    g_client.sendPacket(buf, len);
    if (auto cmd = g_client.receiveCommand()) {
        fmt::print("received command: {}\n", *cmd);
        // TODO: modify or replay logic based on command
    }
    return real_send(s, buf, len, flags);
}

int WSAAPI HookedRecv(SOCKET s, char* buf, int len, int flags) {
    int ret = real_recv(s, buf, len, flags);
    if (ret > 0) {
        fmt::print("recv intercepted: {} bytes\n", ret);
        g_client.sendPacket(buf, ret);
        if (auto cmd = g_client.receiveCommand()) {
            fmt::print("received command: {}\n", *cmd);
            // TODO: modify or replay logic based on command
        }
    }
    return ret;
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD reason, LPVOID) {
    if (reason == DLL_PROCESS_ATTACH) {
        DetourTransactionBegin();
        DetourUpdateThread(GetCurrentThread());
        DetourAttach(&(PVOID&)real_send, HookedSend);
        DetourAttach(&(PVOID&)real_recv, HookedRecv);
        DetourTransactionCommit();
        fmt::print("netstrum_hook loaded\n");
    } else if (reason == DLL_PROCESS_DETACH) {
        DetourTransactionBegin();
        DetourUpdateThread(GetCurrentThread());
        DetourDetach(&(PVOID&)real_send, HookedSend);
        DetourDetach(&(PVOID&)real_recv, HookedRecv);
        DetourTransactionCommit();
        fmt::print("netstrum_hook unloaded\n");
    }
    return TRUE;
}
