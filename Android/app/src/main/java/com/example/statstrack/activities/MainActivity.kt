package com.example.statstrack.activities

import android.content.Context
import android.content.Intent
import android.content.SharedPreferences
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.FrameLayout
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.fragment.app.FragmentTransaction
import androidx.lifecycle.lifecycleScope
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
import com.example.statstrack.helper.apiCalls.AccountService
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch

class MainActivity : AppCompatActivity() {

    private lateinit var layout: FrameLayout

    private lateinit var sharedPref: SharedPreferences
    private val accountService: AccountService by lazy {
        AccountService(this)
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        val settings = InitSettings(this, findViewById(android.R.id.content))
        settings.initScreen()

        sharedPref = this.getSharedPreferences("UserData", Context.MODE_PRIVATE)

        layout = findViewById(R.id.mainActivityFrameLayout)

        refreshToken()
    }

    private fun handleRedirect() {
        when (sharedPref.getString("profileStage", "")) {
            "UserBasics" -> goToUser()
            "User" -> goToProfessional()
            "ProfessionalLife" -> goToPersonal()
            "PersonalLife" -> goToHobbies()
            "Hobbies" -> goToExpenses()
            "Completed" -> goToHomePage()
        }
    }
    private fun refreshToken() {
        lifecycleScope.launch(Dispatchers.IO) {
            accountService.refreshToken() { success ->
                lifecycleScope.launch(Dispatchers.Main) {
                    if (success) {
                        handleRedirect()
                    } else {
                         goToSignIn()
                    }
                }
            }
        }
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
    fun goToCode(email: String) {
        val fragment = CodeFragment.newInstance(email)
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
    fun goToHomePage() {
        val intent = Intent(this, HomeActivity::class.java)
        startActivity(intent)
    }
}
