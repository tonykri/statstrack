package com.example.statstrack.fragments.homepages.searchpage

import android.graphics.Color
import android.os.Bundle
import android.util.TypedValue
import android.view.Gravity
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.LinearLayout
import android.widget.TextView
import androidx.core.content.ContextCompat
import com.example.statstrack.R
import com.example.statstrack.fragments.homepages.SearchPageFragment

class CategoriesNavbarFragment : Fragment() {

    private lateinit var layout: LinearLayout
    private var categoryList: MutableList<TextView> = mutableListOf()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val view = inflater.inflate(R.layout.fragment_categories_navbar, container, false)

        layout = view.findViewById(R.id.categoriesNavbarFragmentLayout)
        initCategories()

        return view
    }

    private fun initTextView(category: String) : TextView {
        var textView = TextView(requireContext())
        textView.text = category
        textView.setTextSize(TypedValue.COMPLEX_UNIT_SP, 18f)
        textView.setTextColor(Color.BLACK)
        val layoutParams = LinearLayout.LayoutParams(
            LinearLayout.LayoutParams.WRAP_CONTENT,
            LinearLayout.LayoutParams.MATCH_PARENT
        )
        layoutParams.gravity = Gravity.CENTER
        layoutParams.setMargins(5, 2, 5, 2)
        textView.layoutParams = layoutParams
        val paddingInPixels = resources.getDimensionPixelSize(R.dimen.eight_padding_dimension)
        textView.setPadding(paddingInPixels, paddingInPixels, paddingInPixels, paddingInPixels)

        textView.setOnClickListener{
            handleClick(textView)
        }

        categoryList.add(textView)

        return textView
    }
    private fun initCategories() {
        val categories: Array<String> = resources.getStringArray(R.array.business_category_array)
        layout.removeAllViews()
        categoryList.clear()

        for (category in categories) {
            val textView = initTextView(category)
            layout.addView(textView)
        }
    }

    private fun handleClick(textView: TextView) {
        val parentFragment = parentFragment
        val textViewColor = textView.currentTextColor
        val orangeColor = ContextCompat.getColor(requireContext(), R.color.orangeDark)
        if(textViewColor == orangeColor) {
            for (category in categoryList) {
                category.setTextColor(ContextCompat.getColor(requireContext(), R.color.black))
            }
            if(parentFragment is SearchPageFragment)
                parentFragment.getCategory()
        }else {
            for (category in categoryList) {
                category.setTextColor(ContextCompat.getColor(requireContext(), R.color.black))
            }
            textView.setTextColor(ContextCompat.getColor(requireContext(), R.color.orangeDark))
            if(parentFragment is SearchPageFragment)
                parentFragment.getCategory(textView.text.toString().replace(" ", "_"))
        }
    }
}