package com.example.statstrack.fragments.auth

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import com.example.statstrack.R
import com.example.statstrack.activities.MainActivity

class CodeFragment : Fragment() {

    private lateinit var next: Button

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val view = inflater.inflate(R.layout.fragment_code, container, false)

        next = view.findViewById(R.id.codeFragmentLoginButton)
        next.setOnClickListener{
            (requireActivity() as? MainActivity)?.goToUser()
        }

        return view
    }

}