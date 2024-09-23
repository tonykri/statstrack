package com.example.statstrack.fragments.homepages

import android.content.Context
import android.content.Intent
import android.content.SharedPreferences
import android.net.Uri
import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.LinearLayout
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.lifecycle.lifecycleScope
import com.example.statstrack.R
import com.example.statstrack.activities.BrowserActivity
import com.example.statstrack.fragments.common.BusinessFragment
import com.example.statstrack.helper.apiCalls.HomePageService
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch

class MyBusinessesPageFragment : Fragment() {

    private lateinit var sharedPref: SharedPreferences

    private lateinit var layout: LinearLayout
    private lateinit var addBusinessBtn: Button

    private val homePageService: HomePageService by lazy {
        HomePageService(requireContext())
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        val view = inflater.inflate(R.layout.fragment_my_businesses_page, container, false)

        sharedPref = requireContext().getSharedPreferences("UserData", Context.MODE_PRIVATE)

        layout = view.findViewById(R.id.myBusinessesPageFragmentLayout)
        addBusinessBtn = view.findViewById(R.id.myBusinessesPageFragmentAddBtn)
        addBusinesses()

        addBusinessBtn.setOnClickListener{
            val url = "http://10.0.2.2:4005/pay?token=${sharedPref.getString("id", "")}"
            val intent = Intent(requireContext(), BrowserActivity::class.java)

            intent.putExtra("url", url)

            startActivity(intent)
        }

        return view
    }

    private fun addBusinesses() {
        val fragmentManager = childFragmentManager
        val fragmentTransaction = fragmentManager.beginTransaction()

        layout.removeAllViews()

        lifecycleScope.launch(Dispatchers.IO) {
            homePageService.getMyBusinesses() { data ->
                lifecycleScope.launch(Dispatchers.Main) {
                    if (data != null) {
                        for (business in data) {
                            Log.d("Bid", business.id.toString())
                            val fragment = BusinessFragment(business)
                            fragmentTransaction.add(R.id.myBusinessesPageFragmentLayout, fragment)
                        }
                        fragmentTransaction.commit()
                    } else {
                        Toast.makeText(requireContext(), "Could not fetch data", Toast.LENGTH_LONG).show()
                    }
                }
            }
        }
    }

}