package com.example.statstrack.fragments.homepages

import android.content.Context
import android.content.Intent
import android.content.SharedPreferences
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.lifecycle.lifecycleScope
import com.bumptech.glide.Glide
import com.bumptech.glide.load.model.GlideUrl
import com.bumptech.glide.load.model.LazyHeaders
import com.example.statstrack.R
import com.example.statstrack.activities.MainActivity
import com.example.statstrack.activities.UpdateUserDataActivity
import com.example.statstrack.helper.apiCalls.HomePageService
import de.hdodenhof.circleimageview.CircleImageView
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch


class ProfilePageFragment : Fragment() {

    private lateinit var sharedPref: SharedPreferences

    private lateinit var firstname: TextView
    private lateinit var lastname: TextView
    private lateinit var email: TextView
    private lateinit var photo: CircleImageView

    private lateinit var userBtn: TextView
    private lateinit var professionalBtn: TextView
    private lateinit var personalBtn: TextView
    private lateinit var hobbiesBtn: TextView
    private lateinit var expensesBtn: TextView

    private lateinit var logoutBtn: TextView

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
        val view = inflater.inflate(R.layout.fragment_profile_page, container, false)

        sharedPref = requireContext().getSharedPreferences("UserData", Context.MODE_PRIVATE)

        firstname = view.findViewById(R.id.profilePageFragmentFirstname)
        lastname = view.findViewById(R.id.profilePageFragmentLastname)
        email = view.findViewById(R.id.profilePageFragmentEmail)
        photo = view.findViewById(R.id.profilePageFragmentPhoto)

        userBtn = view.findViewById(R.id.profilePageFragmentUserData)
        professionalBtn = view.findViewById(R.id.profilePageFragmentProfessionalData)
        personalBtn = view.findViewById(R.id.profilePageFragmentPersonalData)
        hobbiesBtn = view.findViewById(R.id.profilePageFragmentHobbiesData)
        expensesBtn = view.findViewById(R.id.profilePageFragmentExpensesData)

        logoutBtn = view.findViewById(R.id.profilePageFragmentLogout)

        userBtn.setOnClickListener{
            val intent = Intent(requireContext(), UpdateUserDataActivity::class.java).apply {
                putExtra("Fragment", "User")
            }
            startActivity(intent)
        }
        professionalBtn.setOnClickListener{
            val intent = Intent(requireContext(), UpdateUserDataActivity::class.java).apply {
                putExtra("Fragment", "Professional")
            }
            startActivity(intent)
        }
        personalBtn.setOnClickListener{
            val intent = Intent(requireContext(), UpdateUserDataActivity::class.java).apply {
                putExtra("Fragment", "Personal")
            }
            startActivity(intent)
        }
        hobbiesBtn.setOnClickListener{
            val intent = Intent(requireContext(), UpdateUserDataActivity::class.java).apply {
                putExtra("Fragment", "Hobbies")
            }
            startActivity(intent)
        }
        expensesBtn.setOnClickListener{
            val intent = Intent(requireContext(), UpdateUserDataActivity::class.java).apply {
                putExtra("Fragment", "Expenses")
            }
            startActivity(intent)
        }

        logoutBtn.setOnClickListener{
            val editor = sharedPref.edit()
            editor.clear()
            editor.apply()
            val intent = Intent(requireContext(), MainActivity::class.java)
            startActivity(intent)
        }

        initData()

        return view
    }

    private fun initData() {
        lifecycleScope.launch(Dispatchers.IO) {
            homePageService.getUserData() { data ->
                lifecycleScope.launch(Dispatchers.Main) {
                    if (data != null) {
                        firstname.text = data.firstName
                        lastname.text = data.lastName
                        email.text = data.email
                        if (data.photoUrl != null) {
                            val glideUrl = GlideUrl(
                                "your_image_url", LazyHeaders.Builder()
                                    .addHeader("Header-Name", "Header-Value")
                                    .build()
                            )
                            Glide.with(requireContext())
                                .load(glideUrl)
                                .into(photo)
                        }
                    } else {
                        Toast.makeText(requireContext(), "Could not fetch data", Toast.LENGTH_LONG).show()
                    }
                }
            }
        }
    }

}