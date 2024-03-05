package com.example.statstrack.activities

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.FrameLayout
import androidx.fragment.app.Fragment
import androidx.fragment.app.FragmentTransaction
import com.example.statstrack.R
import com.example.statstrack.fragments.common.NavbarFragment
import com.example.statstrack.fragments.pages.MyBusinessesPageFragment
import com.example.statstrack.fragments.pages.SearchPageFragment
import com.example.statstrack.helper.InitSettings

class HomeActivity : AppCompatActivity() {

    private lateinit var pagesLayout: FrameLayout
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_home)

        val settings = InitSettings(this, findViewById(android.R.id.content))
        settings.initScreen()

        pagesLayout = findViewById(R.id.homeActivityPagesLayout)
        replaceFragment(SearchPageFragment())

    }

    private fun replaceFragment(fragment: Fragment) {
        supportFragmentManager.beginTransaction()
            .replace(pagesLayout.id, fragment)
            .commit()
    }

    public fun goToSearchPage() {
        replaceFragment(SearchPageFragment())
    }
    public fun goToMyBusinessesPage() {
        replaceFragment(MyBusinessesPageFragment())
    }
}