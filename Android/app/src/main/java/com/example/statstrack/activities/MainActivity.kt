package com.example.statstrack.activities

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.FrameLayout
import androidx.fragment.app.Fragment
import androidx.fragment.app.FragmentTransaction
import com.example.statstrack.R
import com.example.statstrack.fragments.auth.CodeFragment
import com.example.statstrack.fragments.auth.ExpensesFragment
import com.example.statstrack.fragments.auth.HobbiesFragment
import com.example.statstrack.fragments.auth.LoginFragment
import com.example.statstrack.fragments.auth.PersonalFragment
import com.example.statstrack.fragments.auth.ProfessionalFragment
import com.example.statstrack.fragments.auth.RegisterFragment
import com.example.statstrack.fragments.auth.UserFragment
import com.example.statstrack.helper.InitSettings

class MainActivity : AppCompatActivity() {

    private lateinit var layout: FrameLayout

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        val settings = InitSettings(this, findViewById(android.R.id.content))
        settings.initScreen()

        layout = findViewById(R.id.mainActivityFrameLayout)

        goToSignIn()
    }

    private fun replaceFragment(fragment: Fragment) {
        val transaction: FragmentTransaction = supportFragmentManager.beginTransaction()
        transaction.replace(layout.id, fragment)
        transaction.commit()
    }
    fun goToSignIn() {
        val fragment = LoginFragment()
        replaceFragment(fragment)
    }
    fun goToSignUp() {
        val fragment = RegisterFragment()
        replaceFragment(fragment)
    }
    fun goToCode() {
        val fragment = CodeFragment()
        replaceFragment(fragment)
    }
    fun goToUser() {
        val fragment = UserFragment()
        replaceFragment(fragment)
    }
    fun goToProfessional() {
        val fragment = ProfessionalFragment()
        replaceFragment(fragment)
    }
    fun goToPersonal() {
        val fragment = PersonalFragment()
        replaceFragment(fragment)
    }
    fun goToHobbies() {
        val fragment = HobbiesFragment()
        replaceFragment(fragment)
    }
    fun goToExpenses() {
        val fragment = ExpensesFragment()
        replaceFragment(fragment)
    }
}
