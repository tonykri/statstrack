package com.example.statstrack.fragments.auth

import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.EditText
import android.widget.TextView
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.lifecycle.lifecycleScope
import com.example.statstrack.R
import com.example.statstrack.activities.MainActivity
import com.example.statstrack.helper.apiCalls.AccountService
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.runBlocking

class LoginFragment : Fragment() {

    private lateinit var signUp: TextView
    private lateinit var next: Button
    private lateinit var emailAddress: EditText

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
        val view = inflater.inflate(R.layout.fragment_login, container, false)

        emailAddress = view.findViewById(R.id.loginFragmentEmailAddress)

        signUp = view.findViewById(R.id.loginFragmentRegisterButton)
        signUp.setOnClickListener{
            (requireActivity() as? MainActivity)?.goToSignUp()
        }

        next = view.findViewById(R.id.loginFragmentLoginButton)
        next.setOnClickListener{
            login()
        }

        return view
    }

    private fun login() {
        next.isEnabled = false
        val email = emailAddress.text.toString()
        lifecycleScope.launch(Dispatchers.IO) {
            accountService.login(email) { success ->
                lifecycleScope.launch(Dispatchers.Main) {
                    if (success) {
                        (requireActivity() as? MainActivity)?.goToCode(email)
                        Toast.makeText(requireContext(), "Email has been sent", Toast.LENGTH_SHORT).show()
                    } else {
                        Toast.makeText(requireContext(), "Something is wrong. Try again...", Toast.LENGTH_LONG).show()
                        next.isEnabled = true
                    }
                }
            }
        }
    }

}