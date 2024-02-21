package com.example.statstrack.activities

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import com.example.statstrack.R
import com.example.statstrack.helper.InitSettings

class MainActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        val settings = InitSettings(this, findViewById(android.R.id.content))
        settings.initScreen()
    }
}