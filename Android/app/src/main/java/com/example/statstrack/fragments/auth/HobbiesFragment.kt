package com.example.statstrack.fragments.auth

import android.content.Context
import android.content.SharedPreferences
import android.os.Bundle
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
import com.example.statstrack.activities.MainActivity
import com.example.statstrack.helper.apiCalls.CompleteAccountService
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch

class HobbiesFragment : Fragment() {

    private lateinit var layout: LinearLayout
    private lateinit var next: Button
    private lateinit var itemsNoLabel: TextView

    private var selectedItems = mutableListOf<String>()

    private lateinit var sharedPref: SharedPreferences
    private val completeAccountService: CompleteAccountService by lazy {
        CompleteAccountService(requireContext())
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val view = inflater.inflate(R.layout.fragment_hobbies, container, false)

        sharedPref = requireContext().getSharedPreferences("UserData", Context.MODE_PRIVATE)

        layout = view.findViewById(R.id.hobbiesFragmentLayout)
        itemsNoLabel = view.findViewById(R.id.hobbiesFragmentLeftToChoose)
        initHobbies()

        next = view.findViewById(R.id.hobbiesFragmentNext)
        next.setOnClickListener{
            next()
        }

        return view
    }

    private fun initTextView(hobby: String) : TextView {
        val textView = TextView(requireContext())
        textView.text = hobby
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

        textView.setOnClickListener{
            if (selectedItems.size >= 7)
                return@setOnClickListener
            if (selectedItems.contains(textView.text)) {
                selectedItems.remove(textView.text)
                textView.setBackgroundResource(R.drawable.rounded_background)
                itemsNoLabel.text = (7 - selectedItems.size).toString()
            }else {
                selectedItems.add(textView.text as String)
                textView.setBackgroundResource(R.drawable.rounded_background_selected)
                itemsNoLabel.text = (7 - selectedItems.size).toString()
            }
        }

        return textView
    }
    private fun initHobbies() {
        val hobbies: Array<String> = resources.getStringArray(R.array.hobbies)
        layout.removeAllViews()

        for (hobby in hobbies) {
            val textView = initTextView(hobby)
            layout.addView(textView)
        }
    }

    private fun next() {
        lifecycleScope.launch(Dispatchers.IO) {
            completeAccountService.setHobbies(selectedItems) { success ->
                lifecycleScope.launch(Dispatchers.Main) {
                    if (success) {
                        (requireActivity() as? MainActivity)?.goToExpenses()
                        Toast.makeText(requireContext(), "Data Saved", Toast.LENGTH_SHORT).show()
                    } else {
                        Toast.makeText(requireContext(), "Something is wrong. Try again...", Toast.LENGTH_LONG).show()
                    }
                }
            }
        }
    }
}