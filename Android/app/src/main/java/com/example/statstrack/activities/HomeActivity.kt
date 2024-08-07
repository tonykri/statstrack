package com.example.statstrack.activities

import android.content.Context
import android.content.Intent
import android.Manifest
import android.content.SharedPreferences
import android.content.pm.PackageManager
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.FrameLayout
import androidx.core.app.ActivityCompat
import androidx.core.content.ContextCompat
import androidx.fragment.app.Fragment
import com.example.statstrack.R
import com.example.statstrack.databinding.ActivityMainBinding
import com.example.statstrack.fragments.homepages.CouponsPageFragment
import com.example.statstrack.fragments.homepages.MyBusinessesPageFragment
import com.example.statstrack.fragments.homepages.ProfilePageFragment
import com.example.statstrack.fragments.homepages.SearchPageFragment
import com.example.statstrack.helper.InitSettings
import com.example.statstrack.helper.LocationService
import com.example.statstrack.helper.apiCalls.AccountService
import com.example.statstrack.helper.apiCalls.HomePageService

class HomeActivity : AppCompatActivity() {

    private lateinit var binding: ActivityMainBinding
    private val LOCATION_PERMISSION_REQUEST_CODE = 1

    private lateinit var pagesLayout: FrameLayout

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_home)

        val settings = InitSettings(this, findViewById(android.R.id.content))
        settings.initScreen()

        pagesLayout = findViewById(R.id.homeActivityPagesLayout)
        replaceFragment(SearchPageFragment())


        if (ContextCompat.checkSelfPermission(
                this,
                Manifest.permission.ACCESS_FINE_LOCATION
            ) != PackageManager.PERMISSION_GRANTED) {
            ActivityCompat.requestPermissions(
                this,
                arrayOf(Manifest.permission.ACCESS_FINE_LOCATION),
                LOCATION_PERMISSION_REQUEST_CODE
            )
        } else {
            startLocationService()
        }
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
    public fun goToCouponsPage() {
        replaceFragment(CouponsPageFragment())
    }
    public fun goToProfilePage() {
        replaceFragment(ProfilePageFragment())
    }


    private fun startLocationService() {
        Intent(this, LocationService::class.java).also { intent ->
            startService(intent)
        }
    }

    override fun onRequestPermissionsResult(
        requestCode: Int,
        permissions: Array<out String>,
        grantResults: IntArray
    ) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults)
        if (requestCode == LOCATION_PERMISSION_REQUEST_CODE) {
            if ((grantResults.isNotEmpty() && grantResults[0] == PackageManager.PERMISSION_GRANTED)) {
                startLocationService()
            }
        }
    }

}