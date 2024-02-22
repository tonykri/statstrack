package com.example.statstrack.fragments.auth

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.RadioButton
import com.example.statstrack.R
import com.example.statstrack.activities.MainActivity

class PersonalFragment : Fragment() {

    private lateinit var stayHomeYes: RadioButton
    private lateinit var stayHomeNo: RadioButton
    private lateinit var marriedYes: RadioButton
    private lateinit var marriedNo: RadioButton

    private lateinit var next: Button

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val view = inflater.inflate(R.layout.fragment_personal, container, false)

        stayHomeYes = view.findViewById(R.id.personalFragmentStayHomeYes)
        stayHomeNo = view.findViewById(R.id.personalFragmentStayHomeNo)
        marriedYes = view.findViewById(R.id.personalFragmentMarriedYes)
        marriedNo = view.findViewById(R.id.personalFragmentMarriedNo)

        stayHomeYes.isChecked = true
        stayHomeNo.isChecked = false
        marriedYes.isChecked = true
        marriedNo.isChecked = false

        stayHomeYes.setOnClickListener{
            stayHomeYes.isChecked = true
            stayHomeNo.isChecked = false
        }
        stayHomeNo.setOnClickListener{
            stayHomeYes.isChecked = false
            stayHomeNo.isChecked = true
        }
        marriedYes.setOnClickListener{
            marriedYes.isChecked = true
            marriedNo.isChecked = false
        }
        marriedNo.setOnClickListener{
            marriedYes.isChecked = false
            marriedNo.isChecked = true
        }

        next = view.findViewById(R.id.personalFragmentNext)
        next.setOnClickListener{
            (requireActivity() as? MainActivity)?.goToHobbies()
        }

        return view
    }

}