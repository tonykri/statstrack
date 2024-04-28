package com.example.statstrack.activities

import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.FrameLayout
import androidx.fragment.app.Fragment
import androidx.fragment.app.FragmentTransaction
import com.example.statstrack.R
import com.example.statstrack.fragments.auth.ExpensesFragment
import com.example.statstrack.fragments.auth.HobbiesFragment
import com.example.statstrack.fragments.auth.PersonalFragment
import com.example.statstrack.fragments.auth.ProfessionalFragment
import com.example.statstrack.fragments.auth.UserFragment

class UpdateUserDataActivity : AppCompatActivity() {

    private lateinit var layout: FrameLayout

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_update_user_data)

        layout = findViewById(R.id.updateUserDataActivityLayout)
        val fragmentName = intent.getStringExtra("Fragment")
        changeLayout(fragmentName)
    }

    private fun replaceFragment(fragment: Fragment) {
        val transaction: FragmentTransaction = supportFragmentManager.beginTransaction()
        transaction.replace(layout.id, fragment)
        transaction.commit()
    }
    private fun changeLayout(fragmentName: String?) {
        when (fragmentName) {
            "User" -> {
                val fragment = UserFragment(true)
                replaceFragment(fragment)
            }
            "Professional" -> {
                val fragment = ProfessionalFragment(true)
                replaceFragment(fragment)
            }
            "Personal" -> {
                val fragment = PersonalFragment(true)
                replaceFragment(fragment)
            }
            "Hobbies" -> {
                val fragment = HobbiesFragment(true)
                replaceFragment(fragment)
            }
            "Expenses" -> {
                val fragment = ExpensesFragment(true)
                replaceFragment(fragment)
            }
        }
    }
}