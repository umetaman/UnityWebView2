#include <Windows.h>
#include <WebView2.h>
#include <string>
#include <tchar.h>
#include <vector>
#include <wrl.h>
#include <wil/com.h>

#define PLUGIN_API extern "C" __declspec(dllexport)

using namespace Microsoft::WRL;

static wil::com_ptr<ICoreWebView2Controller> webviewController;
static wil::com_ptr<ICoreWebView2> webviewWindow;

PLUGIN_API void createWebView(LPCWSTR url) {
    HWND hWnd = GetActiveWindow();

	if (hWnd == NULL) {
		return;
	}

    //https://docs.microsoft.com/ja-jp/microsoft-edge/webview2/gettingstarted/win32
    CreateCoreWebView2EnvironmentWithOptions(nullptr, nullptr, nullptr,
        Callback<ICoreWebView2CreateCoreWebView2EnvironmentCompletedHandler>(
            [hWnd, url](HRESULT result, ICoreWebView2Environment* env) -> HRESULT {

                // Create a CoreWebView2Controller and get the associated CoreWebView2 whose parent is the main window hWnd
                env->CreateCoreWebView2Controller(hWnd, Callback<ICoreWebView2CreateCoreWebView2ControllerCompletedHandler>(
                    [hWnd, url](HRESULT result, ICoreWebView2Controller* controller) -> HRESULT {
                        if (controller != nullptr) {
                            webviewController = controller;
                            webviewController->get_CoreWebView2(&webviewWindow);
                        }

                        // WebViewの設定
                        ICoreWebView2Settings* Settings;
                        webviewWindow->get_Settings(&Settings);
                        Settings->put_IsScriptEnabled(TRUE);
                        Settings->put_AreDefaultScriptDialogsEnabled(TRUE);
                        Settings->put_IsWebMessageEnabled(TRUE);

                        // 最初は親のウィンドウに合わせる
                        RECT bounds;
                        GetClientRect(hWnd, &bounds);
                        webviewController->put_Bounds(bounds);
                        
                        // URLを指定して表示
                        webviewWindow->Navigate(url);

                        return S_OK;
                    }).Get());
                return S_OK;
            }).Get());
}

PLUGIN_API void navigate(LPCWSTR url) {
    if (webviewWindow != nullptr) {
        webviewWindow->Navigate(url);
    }
}

PLUGIN_API void navigateToHTML(LPCWSTR htmlContent) {
    if (webviewWindow != nullptr) {
        webviewWindow->NavigateToString(htmlContent);
    }
}

PLUGIN_API void updateWebViewBound(int x, int y, int width, int height) {
    if (webviewController != nullptr) {
        RECT bounds;
        bounds.left = x;
        bounds.top = y;
        bounds.right = x + width;
        bounds.bottom = y + height;

        webviewController->put_Bounds(bounds);
    }
}

PLUGIN_API void closeWebView() {
    if (webviewController != nullptr) {
        webviewController->Close();
        webviewController = nullptr;
        webviewWindow = nullptr;
    }
}

PLUGIN_API bool isActive() {
    return webviewController != nullptr;
}