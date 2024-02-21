package com.example.statstrack.helper

import android.content.pm.ActivityInfo
import android.graphics.Color
import android.view.View
import androidx.appcompat.app.AppCompatActivity

class InitSettings(private val activity: AppCompatActivity, private val rootView: View): IInitSettings {
    override fun initScreen() {
        activity.requestedOrientation = ActivityInfo.SCREEN_ORIENTATION_PORTRAIT
        activity.supportActionBar?.hide()
        rootView.setBackgroundColor(Color.WHITE)
    }
}
