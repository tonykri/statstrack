package com.example.statstrack.fragments.auth

import android.content.Context
import android.content.SharedPreferences
import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.EditText
import android.widget.TextView
import android.widget.Toast
import androidx.lifecycle.lifecycleScope
import com.example.statstrack.R
import com.example.statstrack.activities.MainActivity
import com.example.statstrack.helper.apiCalls.AccountService
import com.example.statstrack.helper.apiCalls.CompleteAccountService
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch

class RegisterFragment : Fragment() {

    private lateinit var signIn: TextView
    private lateinit var next: Button

    private lateinit var firstNameEditText: EditText
    private lateinit var lastNameEditText: EditText
    private lateinit var emailEditText: EditText

    private lateinit var sharedPref: SharedPreferences
    private val accountService: AccountService by lazy {
        AccountService(requireContext())
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val view = inflater.inflate(R.layout.fragment_register, container, false)

        sharedPref = requireContext().getSharedPreferences("UserData", Context.MODE_PRIVATE)

        firstNameEditText = view.findViewById(R.id.registerFragmentFirstName)
        lastNameEditText = view.findViewById(R.id.registerFragmentLastName)
        emailEditText = view.findViewById(R.id.registerFragmentEmailAddress)

        signIn = view.findViewById(R.id.registerFragmentLoginButton)
        signIn.setOnClickListener{
            (requireActivity() as? MainActivity)?.goToSignIn()
        }
        next = view.findViewById(R.id.registerFragmentRegisterButton)
        next.setOnClickListener{
            register()
        }

        return view
    }

    private fun register() {
        val email = emailEditText.text.toString()
        val firstName = firstNameEditText.text.toString()
        val lastName = lastNameEditText.text.toString()
        lifecycleScope.launch(Dispatchers.IO) {
            accountService.register(email, firstName, lastName) { success ->
                lifecycleScope.launch(Dispatchers.Main) {
                    if (success) {
                        (requireActivity() as? MainActivity)?.goToCode(email)
                        Toast.makeText(requireContext(), "Email has been sent", Toast.LENGTH_SHORT).show()
                    } else {
                        Toast.makeText(requireContext(), "Something is wrong. Try again...", Toast.LENGTH_LONG).show()
                    }
                }
            }
        }
    }

}