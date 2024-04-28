package com.example.statstrack.fragments.auth

import android.content.Context
import android.content.SharedPreferences
import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ArrayAdapter
import android.widget.Button
import android.widget.Spinner
import android.widget.Toast
import androidx.lifecycle.lifecycleScope
import com.example.statstrack.R
import com.example.statstrack.activities.MainActivity
import com.example.statstrack.helper.apiCalls.CompleteAccountService
import com.example.statstrack.helper.apiCalls.UpdateAccountService
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch

class ProfessionalFragment(private var update: Boolean = false) : Fragment() {

    private lateinit var eduLevel: Spinner
    private lateinit var industry: Spinner
    private lateinit var income: Spinner
    private lateinit var workHours: Spinner

    private lateinit var sharedPref: SharedPreferences
    private val completeAccountService: CompleteAccountService by lazy {
        CompleteAccountService(requireContext())
    }
    private val updateAccountService: UpdateAccountService by lazy {
        UpdateAccountService(requireContext())
    }

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

        sharedPref = requireContext().getSharedPreferences("UserData", Context.MODE_PRIVATE)

        next = view.findViewById(R.id.professionalFragmentNext)
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
        val eduLevel = eduLevel.selectedItem.toString()
        val industry = industry.selectedItem.toString()
        val income = income.selectedItem.toString()
        val workingHours = workHours.selectedItem.toString()

        next.isEnabled = false

        lifecycleScope.launch(Dispatchers.IO) {
            completeAccountService.setProfessionalData(eduLevel, industry, income, workingHours) { success ->
                lifecycleScope.launch(Dispatchers.Main) {
                    if (success) {
                        (requireActivity() as? MainActivity)?.goToPersonal()
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
        val eduLevel = eduLevel.selectedItem.toString()
        val industry = industry.selectedItem.toString()
        val income = income.selectedItem.toString()
        val workingHours = workHours.selectedItem.toString()

        next.isEnabled = false

        lifecycleScope.launch(Dispatchers.IO) {
            updateAccountService.updateProfessionalData(eduLevel, industry, income, workingHours) { success ->
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