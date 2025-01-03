package com.example.statstrack.activities

import android.annotation.SuppressLint
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.util.Log
import android.webkit.WebResourceError
import android.webkit.WebResourceRequest
import android.webkit.WebSettings
import android.webkit.WebView
import android.webkit.WebViewClient
import com.example.statstrack.R

class BrowserActivity : AppCompatActivity() {
    private lateinit var webView: WebView

    @SuppressLint("SetJavaScriptEnabled")
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_browser)

        webView = findViewById(R.id.browserActivityWebView)

        // Add WebView settings configuration
        webView.settings.apply {
            javaScriptEnabled = true
            domStorageEnabled = true  // Enable DOM storage
            allowContentAccess = true
            allowFileAccess = true
            // For HTTPS pages
            mixedContentMode = WebSettings.MIXED_CONTENT_ALWAYS_ALLOW
        }

        // Custom WebViewClient to handle errors
        webView.webViewClient = object : WebViewClient() {
            override fun onReceivedError(
                view: WebView?,
                request: WebResourceRequest?,
                error: WebResourceError?
            ) {
                super.onReceivedError(view, request, error)
                // Handle errors here
                Log.e("WebView", "Error: ${error?.description}")
            }
        }

        val url = intent.getStringExtra("url") ?: ""
        if (url.isNotEmpty()) {
            webView.loadUrl(url)
        }
    }
}