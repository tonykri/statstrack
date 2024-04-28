package com.example.statstrack.fragments.auth

import android.content.Context
import android.content.SharedPreferences
import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.RadioButton
import android.widget.Toast
import androidx.lifecycle.lifecycleScope
import com.example.statstrack.R
import com.example.statstrack.activities.MainActivity
import com.example.statstrack.helper.apiCalls.CompleteAccountService
import com.example.statstrack.helper.apiCalls.UpdateAccountService
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch

class PersonalFragment(private var update: Boolean = false) : Fragment() {

    private lateinit var stayHomeYes: RadioButton
    private lateinit var stayHomeNo: RadioButton
    private lateinit var marriedYes: RadioButton
    private lateinit var marriedNo: RadioButton

    private lateinit var next: Button

    private lateinit var sharedPref: SharedPreferences
    private val completeAccountService: CompleteAccountService by lazy {
        CompleteAccountService(requireContext())
    }
    private val updateAccountService: UpdateAccountService by lazy {
        UpdateAccountService(requireContext())
    }

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

        sharedPref = requireContext().getSharedPreferences("UserData", Context.MODE_PRIVATE)

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
        if (update)
            next.text = requireContext().getString(R.string.update)
        next.setOnClickListener{
            if (update)
                update()
            else
                next()
        }

        return view
    }

    private fun next() {
        val married = marriedYes.isChecked
        val stayHome = stayHomeYes.isChecked

        next.isEnabled = false

        lifecycleScope.launch(Dispatchers.IO) {
            completeAccountService.setPersonalData(married, stayHome) { success ->
                lifecycleScope.launch(Dispatchers.Main) {
                    if (success) {
                        (requireActivity() as? MainActivity)?.goToHobbies()
                        Toast.makeText(requireContext(), "Data Saved", Toast.LENGTH_SHORT).show()
                    } else {
                        Toast.makeText(requireContext(), "Something is wrong. Try again...", Toast.LENGTH_LONG).show()
                        next.isEnabled = true
                    }
                }
            }
        }
    }

    private fun update() {
        val married = marriedYes.isChecked
        val stayHome = stayHomeYes.isChecked

        next.isEnabled = false

        lifecycleScope.launch(Dispatchers.IO) {
            updateAccountService.updatePersonalData(married, stayHome) { success ->
                lifecycleScope.launch(Dispatchers.Main) {
                    if (success) {
                        next.isEnabled = true
                        Toast.makeText(requireContext(), "Data Saved", Toast.LENGTH_SHORT).show()
                    } else {
                        Toast.makeText(requireContext(), "Something is wrong. Try again...", Toast.LENGTH_LONG).show()
                        next.isEnabled = true
                    }
                }
            }
        }
    }

}