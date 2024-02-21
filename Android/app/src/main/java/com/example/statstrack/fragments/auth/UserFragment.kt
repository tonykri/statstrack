package com.example.statstrack.fragments.auth

import android.app.DatePickerDialog
import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import com.example.statstrack.R
import java.util.Calendar

class UserFragment : Fragment() {

    private lateinit var birthdate: TextView

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

        birthdate.setOnClickListener{
            showDatePickerDialog()
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

}