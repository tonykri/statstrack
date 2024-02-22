package com.example.statstrack.fragments.auth

import android.app.DatePickerDialog
import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ArrayAdapter
import android.widget.Button
import android.widget.Spinner
import android.widget.TextView
import com.example.statstrack.R
import com.example.statstrack.activities.MainActivity
import java.util.Calendar

class UserFragment : Fragment() {

    private lateinit var birthdate: TextView
    private lateinit var phoneCode: Spinner
    private lateinit var gender: Spinner
    private lateinit var country: Spinner

    private lateinit var next: Button

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        val view = inflater.inflate(R.layout.fragment_user, container, false)

        birthdate = view.findViewById(R.id.userFragmentBirthdate)

        initGender(view)
        initCountries(view)
        initPhoneCode(view)

        birthdate.setOnClickListener{
            showDatePickerDialog()
        }

        next = view.findViewById(R.id.userFragmentNext)
        next.setOnClickListener{
            (requireActivity() as? MainActivity)?.goToProfessional()
        }

        return view
    }

    private fun showDatePickerDialog() {
        val calendar = Calendar.getInstance()
        val year = calendar.get(Calendar.YEAR)
        val month = calendar.get(Calendar.MONTH)
        val day = calendar.get(Calendar.DAY_OF_MONTH)

        val datePickerDialog = DatePickerDialog(
            requireContext(),
            { _, selectedYear, selectedMonth, selectedDay ->
                var month = (selectedMonth + 1).toString()
                if (selectedMonth + 1 < 10)
                    month = 0.toString() + (selectedMonth + 1).toString()
                var day = (selectedDay).toString()
                if (selectedDay < 10)
                    day = 0.toString() + (selectedDay).toString()
                val selectedDate = "$selectedYear-${month}-$day"
                birthdate.text = selectedDate
            },
            year,
            month,
            day
        )
        datePickerDialog.show()
    }
    private fun initPhoneCode(view: View) {
        phoneCode = view.findViewById(R.id.userFragmentPhoneCode)

        val data: Array<String> = resources.getStringArray(R.array.country_codes)
        val adapter = ArrayAdapter(
            requireContext(),
            android.R.layout.simple_spinner_item,
            data
        )
        adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item)
        phoneCode.adapter = adapter
    }
    private fun initGender(view: View) {
        gender = view.findViewById(R.id.userFragmentGender)

        val data: Array<String> = resources.getStringArray(R.array.genders)
        val adapter = ArrayAdapter(
            requireContext(),
            android.R.layout.simple_spinner_item,
            data
        )
        adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item)
        gender.adapter = adapter
    }
    private fun initCountries(view: View) {
        country = view.findViewById(R.id.userFragmentCountries)

        val data: Array<String> = resources.getStringArray(R.array.country_names)
        val adapter = ArrayAdapter(
            requireContext(),
            android.R.layout.simple_spinner_item,
            data
        )
        adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item)
        country.adapter = adapter
    }

}