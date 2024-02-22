package com.example.statstrack.fragments.auth

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ArrayAdapter
import android.widget.Button
import android.widget.Spinner
import com.example.statstrack.R
import com.example.statstrack.activities.MainActivity

class ProfessionalFragment : Fragment() {

    private lateinit var eduLevel: Spinner
    private lateinit var industry: Spinner
    private lateinit var income: Spinner
    private lateinit var workHours: Spinner

    private lateinit var next: Button

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val view = inflater.inflate(R.layout.fragment_professional, container, false)

        initEduLevel(view)
        initIncome(view)
        initIndustry(view)
        initWorkHours(view)

        next = view.findViewById(R.id.professionalFragmentNext)
        next.setOnClickListener{
            (requireActivity() as? MainActivity)?.goToPersonal()
        }

        return view
    }

    private fun initEduLevel(view: View) {
        eduLevel = view.findViewById(R.id.professionalFragmentEduLevel)

        val data: Array<String> = resources.getStringArray(R.array.education_levels)
        val adapter = ArrayAdapter(
            requireContext(),
            android.R.layout.simple_spinner_item,
            data
        )
        adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item)
        eduLevel.adapter = adapter
    }
    private fun initIndustry(view: View) {
        industry = view.findViewById(R.id.professionalFragmentIndustry)

        val data: Array<String> = resources.getStringArray(R.array.professional_sectors)
        val adapter = ArrayAdapter(
            requireContext(),
            android.R.layout.simple_spinner_item,
            data
        )
        adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item)
        industry.adapter = adapter
    }
    private fun initIncome(view: View) {
        income = view.findViewById(R.id.professionalFragmentIncome)

        val data: Array<String> = resources.getStringArray(R.array.income_categories)
        val adapter = ArrayAdapter(
            requireContext(),
            android.R.layout.simple_spinner_item,
            data
        )
        adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item)
        income.adapter = adapter
    }
    private fun initWorkHours(view: View) {
        workHours = view.findViewById(R.id.professionalFragmentWorkHours)

        val data: Array<String> = resources.getStringArray(R.array.employment_types)
        val adapter = ArrayAdapter(
            requireContext(),
            android.R.layout.simple_spinner_item,
            data
        )
        adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item)
        workHours.adapter = adapter
    }

}