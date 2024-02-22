package com.example.statstrack.fragments.auth

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.TextView
import com.example.statstrack.R
import com.example.statstrack.activities.MainActivity

class RegisterFragment : Fragment() {

    private lateinit var signIn: TextView
    private lateinit var next: Button

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val view = inflater.inflate(R.layout.fragment_register, container, false)

        signIn = view.findViewById(R.id.registerFragmentLoginButton)
        signIn.setOnClickListener{
            (requireActivity() as? MainActivity)?.goToSignIn()
        }
        next = view.findViewById(R.id.registerFragmentRegisterButton)
        next.setOnClickListener{
            (requireActivity() as? MainActivity)?.goToCode()
        }

        return view
    }

}