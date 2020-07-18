using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace WebView2Unity
{
    public static class WebView2Native
    {
        private const string PLUGIN = "WebView2Plugin";
        [DllImport(PLUGIN, CharSet = CharSet.Unicode)]
        private static extern void createWebView(string url);
        [DllImport(PLUGIN, CharSet = CharSet.Unicode)]
        private static extern void navigate(string url);
        [DllImport(PLUGIN, CharSet = CharSet.Unicode)]
        private static extern void navigateToHTML(string htmlContent);
        [DllImport(PLUGIN)]
        private static extern void updateWebViewBound(int x, int y, int width, int height);
        [DllImport(PLUGIN)]
        private static extern void closeWebView();
        [DllImport(PLUGIN)]
        private static extern bool isActive();

        public static void Create(string url)
        {
            createWebView(url);
        }

        public static void Navigate(string url)
        {
            navigate(url);
        }

        public static void NavigateToHTML(string htmlContent)
        {
            navigateToHTML(htmlContent);
        }

        public static void UpdateBound(int x, int y, int width, int height)
        {
            updateWebViewBound(x, y, width, height);
        }

        public static void UpdateBound(Rect rect)
        {
            UpdateBound((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
        }

        public static void Close()
        {
            closeWebView();
        }

        public static bool IsActive()
        {
            return isActive();
        }
    }

    public static class UIHelper
    {
        public static Rect GetCanvasRectangle(RectTransform rectTransform)
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);

            return new Rect(
                corners[1].x, Screen.height - corners[1].y,
                corners[3].x - corners[1].x,
                corners[1].y - corners[3].y
                );
        }
    }
}