package com.example.statstrack.fragments.auth

import android.content.Intent
import android.os.Bundle
import android.view.Gravity
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.LinearLayout
import android.widget.TextView
import com.example.statstrack.R
import com.example.statstrack.activities.HomeActivity

class ExpensesFragment : Fragment() {

    private lateinit var layout: LinearLayout
    private lateinit var next: Button

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val view = inflater.inflate(R.layout.fragment_expenses, container, false)

        layout = view.findViewById(R.id.expensesFragmentLayout)
        initExpenses()

        next = view.findViewById(R.id.expensesFragmentNext)
        next.setOnClickListener{
            val intent = Intent(requireContext(), HomeActivity::class.java)
            startActivity(intent)
        }

        return view
    }

    private fun initTextView(expense: String) : TextView {
        var textView = TextView(requireContext())
        textView.text = expense
        textView.setTextAppearance(requireContext(), R.style.ButtonStyle)
        val layoutParams = LinearLayout.LayoutParams(
            LinearLayout.LayoutParams.WRAP_CONTENT,
            LinearLayout.LayoutParams.WRAP_CONTENT
        )
        layoutParams.gravity = Gravity.CENTER
        layoutParams.setMargins(2, 2, 2, 2)
        textView.layoutParams = layoutParams
        textView.setBackgroundResource(R.drawable.rounded_background)
        val paddingInPixels = resources.getDimensionPixelSize(R.dimen.eight_padding_dimension)
        textView.setPadding(paddingInPixels, paddingInPixels, paddingInPixels, paddingInPixels)

        return textView
    }
    private fun initExpenses() {
        val expenses: Array<String> = resources.getStringArray(R.array.expenses)
        layout.removeAllViews()

        for (expense in expenses) {
            val textView = initTextView(expense)
            layout.addView(textView)
        }
    }

}
