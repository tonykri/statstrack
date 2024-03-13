package com.example.statstrack.fragments.homepages

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.LinearLayout
import androidx.fragment.app.Fragment
import com.example.statstrack.R
import com.example.statstrack.fragments.common.BusinessFragment

class MyBusinessesPageFragment : Fragment() {

    private lateinit var layout: LinearLayout

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        val view = inflater.inflate(R.layout.fragment_my_businesses_page, container, false)

        layout = view.findViewById(R.id.myBusinessesPageFragmentLayout)
        addBusinesses()

        return view
    }

    private fun addBusinesses() {
        val fragmentManager = childFragmentManager
        val fragmentTransaction = fragmentManager.beginTransaction()

        layout.removeAllViews()

        val fragment1 = BusinessFragment()
        val fragment2 = BusinessFragment()
        val fragment3 = BusinessFragment()
        fragmentTransaction.add(R.id.myBusinessesPageFragmentLayout, fragment1)
        fragmentTransaction.add(R.id.myBusinessesPageFragmentLayout, fragment2)
        fragmentTransaction.add(R.id.myBusinessesPageFragmentLayout, fragment3)

        fragmentTransaction.commit()
    }

}