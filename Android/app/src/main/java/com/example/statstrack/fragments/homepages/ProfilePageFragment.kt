package com.example.statstrack.fragments.homepages

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
import com.example.statstrack.helper.apiCalls.HomePageService
import de.hdodenhof.circleimageview.CircleImageView
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch


class ProfilePageFragment : Fragment() {

    private lateinit var firstname: TextView
    private lateinit var lastname: TextView
    private lateinit var email: TextView
    private lateinit var photo: CircleImageView

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

        firstname = view.findViewById(R.id.profilePageFragmentFirstname)
        lastname = view.findViewById(R.id.profilePageFragmentLastname)
        email = view.findViewById(R.id.profilePageFragmentEmail)
        photo = view.findViewById(R.id.profilePageFragmentPhoto)

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