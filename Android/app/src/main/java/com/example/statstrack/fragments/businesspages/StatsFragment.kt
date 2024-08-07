package com.example.statstrack.fragments.businesspages

import android.app.DatePickerDialog
import android.app.TimePickerDialog
import android.content.Context
import android.graphics.Typeface
import android.os.Bundle
import android.util.Log
import android.view.Gravity
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.LinearLayout
import android.widget.TextView
import android.widget.Toast
import androidx.lifecycle.lifecycleScope
import com.example.statstrack.R
import com.example.statstrack.helper.apiCalls.StatsService
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import java.time.LocalDateTime
import java.time.format.DateTimeFormatter
import java.util.Calendar
import java.util.UUID
import kotlin.reflect.full.memberProperties

class StatsFragment(private val businessId: UUID) : Fragment() {

    private lateinit var layout: LinearLayout
    private lateinit var from: TextView
    private lateinit var to: TextView
    private lateinit var searchBtn: Button

    private var fromSelected = false
    private var toSelected = false

    private val statsService: StatsService by lazy {
        StatsService(requireContext())
    }
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        val view = inflater.inflate(R.layout.fragment_stats, container, false)

        layout = view.findViewById(R.id.statsFragmentLayout)
        from = view.findViewById(R.id.statsFragmentFrom)
        to = view.findViewById(R.id.statsFragmentTo)
        searchBtn = view.findViewById(R.id.statsFragmentSearchBtn)
        initStats()

        from.setOnClickListener {
            showDateTimePicker(requireContext()) { selectedDateTime ->
                val formatter = DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm")
                from.text = selectedDateTime.format(formatter)
                fromSelected = true
            }
        }
        to.setOnClickListener {
            showDateTimePicker(requireContext()) { selectedDateTime ->
                val formatter = DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm")
                to.text = selectedDateTime.format(formatter)
                toSelected = true
            }
        }
        searchBtn.setOnClickListener{
            getStats()
        }


        return view
    }

    private fun initStats() {
        layout.removeAllViews()
    }

    private fun getStats() {
        if (!fromSelected && !toSelected) {
            Toast.makeText(requireContext(), "Select dates", Toast.LENGTH_LONG)
                .show()
            return
        }

        val formatter = DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm")
        val localDateTimeFrom = LocalDateTime.parse(from.text.toString(), formatter)
        val localDateTimeTo = LocalDateTime.parse(to.text.toString(), formatter)
        layout.removeAllViews()
        searchBtn.isEnabled = false
        lifecycleScope.launch(Dispatchers.IO) {
            statsService.getBusinessStats(
                businessId,
                localDateTimeFrom,
                localDateTimeTo
            ) { statsResponse ->
                lifecycleScope.launch(Dispatchers.Main) {
                    if (statsResponse != null) {
                        val totalUsers = statsResponse.userStats.sumTotalUsers
                        statsResponse::class.memberProperties.forEach { prop ->
                            handleTitle(prop.name)
                            val value = prop.call(statsResponse)
                            value?.let {
                                it::class.memberProperties.forEach { subProp ->
                                    val subValue = subProp.call(it)
                                    handleProps(subProp.name, subValue.toString().toInt(), totalUsers)
                                }
                            }
                        }

                    } else {
                        Toast.makeText(requireContext(), "Could not fetch data", Toast.LENGTH_LONG)
                            .show()
                    }
                    searchBtn.isEnabled = true
                }
            }
        }
    }

    private fun capitalizeAndInsertSpaces(input: String): String {
        val result = StringBuilder()
        var isFirst = true
        input.forEach { char ->
            if (char.isUpperCase() && !isFirst) {
                result.append(" ")
            }
            result.append(char)
            isFirst = false
        }
        return result.toString().replaceFirstChar { it.uppercase() }
    }

    private fun formatString(input: String): String {
        val formattedWords = input
            .split("(?=[A-Z])".toRegex())
            .joinToString(separator = " ") { it.replaceFirstChar { char -> char.uppercase() } }

        // Handle special cases
        return when {
            formattedWords.startsWith("Sum ") -> formattedWords.substring(4) // Remove "Sum " if exists
            else -> formattedWords // Otherwise, return as it is
        }
    }


    private fun handleTitle(input: String) {
        val title = capitalizeAndInsertSpaces(input)

        val textView = TextView(context)
        textView.id = R.id.textView33
        textView.layoutParams = ViewGroup.LayoutParams(
            ViewGroup.LayoutParams.MATCH_PARENT,
            ViewGroup.LayoutParams.WRAP_CONTENT
        )
        textView.gravity = Gravity.CENTER
        textView.textSize = 20f // 20dp
        textView.setTypeface(null, Typeface.BOLD)
        textView.text = title
        textView.setPadding(10, 10, 10, 10)
        layout.addView(textView)
    }
    private  fun handleProps(input: String, noUsers: Int, totalUsers: Int) {
        val title = formatString(input)
        val output = title + ": " + noUsers*100/totalUsers + "% (" + noUsers + ")"

        val textView = TextView(context)
        textView.id = R.id.textView34
        textView.layoutParams = ViewGroup.LayoutParams(
            ViewGroup.LayoutParams.MATCH_PARENT,
            ViewGroup.LayoutParams.WRAP_CONTENT
        )
        textView.textSize = 18f // 18dp
        textView.text = output
        textView.setPadding(10, 0, 10, 0)
        layout.addView(textView)
    }
    private fun showDatePicker(context: Context, onDateSelected: (LocalDateTime) -> Unit) {
        val calendar = Calendar.getInstance()
        val year = calendar.get(Calendar.YEAR)
        val month = calendar.get(Calendar.MONTH)
        val day = calendar.get(Calendar.DAY_OF_MONTH)

        val datePickerDialog = DatePickerDialog(context, { _, selectedYear, selectedMonth, selectedDay ->
            val selectedDate = LocalDateTime.of(selectedYear, selectedMonth + 1, selectedDay, 0, 0)
            onDateSelected(selectedDate)
        }, year, month, day)

        datePickerDialog.show()
    }
    private fun showTimePicker(context: Context, initialDateTime: LocalDateTime, onTimeSelected: (LocalDateTime) -> Unit) {
        val initialHour = initialDateTime.hour
        val initialMinute = initialDateTime.minute

        val timePickerDialog = TimePickerDialog(context, { _, selectedHour, selectedMinute ->
            val selectedDateTime = initialDateTime.withHour(selectedHour).withMinute(selectedMinute)
            onTimeSelected(selectedDateTime)
        }, initialHour, initialMinute, true)

        timePickerDialog.show()
    }
    private fun showDateTimePicker(context: Context, onDateTimeSelected: (LocalDateTime) -> Unit) {
        showDatePicker(context) { selectedDate ->
            showTimePicker(context, selectedDate) { selectedDateTime ->
                onDateTimeSelected(selectedDateTime)
            }
        }
    }



}