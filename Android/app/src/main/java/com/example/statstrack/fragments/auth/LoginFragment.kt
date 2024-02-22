package com.example.statstrack.fragments.auth

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.TextView
import androidx.fragment.app.Fragment
import com.example.statstrack.R
import com.example.statstrack.activities.MainActivity

class LoginFragment : Fragment() {

    private lateinit var signUp: TextView
    private lateinit var next: Button

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val view = inflater.inflate(R.layout.fragment_login, container, false)

        signUp = view.findViewById(R.id.loginFragmentRegisterButton)
        signUp.setOnClickListener{
            (requireActivity() as? MainActivity)?.goToSignUp()
        }

        next = view.findViewById(R.id.loginFragmentLoginButton)
        next.setOnClickListener{
            (requireActivity() as? MainActivity)?.goToCode()
        }

        return view
    }

}