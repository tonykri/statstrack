package com.example.statstrack.activities

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.FrameLayout
import androidx.fragment.app.FragmentTransaction
import com.example.statstrack.R
import com.example.statstrack.fragments.common.NavbarFragment
import com.example.statstrack.helper.InitSettings

class HomeActivity : AppCompatActivity() {

    private lateinit var navLayout: FrameLayout
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_home)

        val settings = InitSettings(this, findViewById(android.R.id.content))
        settings.initScreen()

        navLayout = findViewById(R.id.homeActivityNavLayout)
        val transaction: FragmentTransaction = supportFragmentManager.beginTransaction()
        val navFragment = NavbarFragment()
        transaction.replace(navLayout.id, navFragment)
        transaction.commit()
    }
}