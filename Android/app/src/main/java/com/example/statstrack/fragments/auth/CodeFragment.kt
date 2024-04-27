package com.example.statstrack.fragments.auth

import android.content.Context
import android.content.SharedPreferences
import android.os.Bundle
import android.os.Handler
import android.provider.ContactsContract.CommonDataKinds.Email
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
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch

class CodeFragment : Fragment() {
    private lateinit var email: String
    private lateinit var sharedPref: SharedPreferences
    companion object {
        private const val ARG_EMAIL = "email"
        fun newInstance(email: String): CodeFragment {
            val fragment = CodeFragment()
            val args = Bundle().apply {
                putString(ARG_EMAIL, email)
            }
            fragment.arguments = args
            return fragment
        }
    }

    private lateinit var next: Button
    private lateinit var code: EditText
    private lateinit var expirationTime: TextView

    private lateinit var handler: Handler
    private lateinit var runnable: Runnable
    private var secondsRemaining = 120

    private val accountService: AccountService by lazy {
        AccountService(requireContext())
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        arguments?.let {
            email = it.getString(ARG_EMAIL).toString()
        }
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val view = inflater.inflate(R.layout.fragment_code, container, false)

        sharedPref = requireContext().getSharedPreferences("UserData", Context.MODE_PRIVATE)

        code = view.findViewById(R.id.codeFragmentCode)
        expirationTime = view.findViewById(R.id.codeFragmentExpirationTime)
        handler = Handler()
        runnable = object : Runnable {
            override fun run() {
                // Update text in TextView
                expirationTime.text = "$secondsRemaining seconds remaining"
                secondsRemaining--
                if (secondsRemaining >= 0) {
                    handler.postDelayed(this, 1000)
                } else {
                    // Countdown complete
                    expirationTime.text = "Code expired!"
                }
            }
        }
        handler.post(runnable)

        next = view.findViewById(R.id.codeFragmentLoginButton)
        next.setOnClickListener{
            login()
        }

        return view
    }

    override fun onDestroy() {
        handler.removeCallbacks(runnable)
        super.onDestroy()
    }

    private fun login() {
        next.isEnabled = false
        val code = code.text.toString()
        lifecycleScope.launch(Dispatchers.IO) {
            accountService.login(email, code) { success ->
                lifecycleScope.launch(Dispatchers.Main) {
                    if (success) {
                        handleRedirect()
                        Toast.makeText(requireContext(), "Login Completed", Toast.LENGTH_SHORT).show()
                    } else {
                        Toast.makeText(requireContext(), "Something is wrong. Try again...", Toast.LENGTH_LONG).show()
                        next.isEnabled = true
                    }
                }
            }
        }
    }

    private fun handleRedirect() {
        when (sharedPref.getString("profileStage", "")) {
            "UserBasics" -> (requireActivity() as? MainActivity)?.goToUser()
            "User" -> (requireActivity() as? MainActivity)?.goToProfessional()
            "ProfessionalLife" -> (requireActivity() as? MainActivity)?.goToPersonal()
            "PersonalLife" -> (requireActivity() as? MainActivity)?.goToHobbies()
            "Hobbies" -> (requireActivity() as? MainActivity)?.goToExpenses()
            "Completed" -> (requireActivity() as? MainActivity)?.goToHomePage()
        }
    }

}