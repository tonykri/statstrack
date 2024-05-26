package com.example.statstrack.fragments.businesspages.editpage

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.ImageView
import android.widget.Toast
import androidx.lifecycle.lifecycleScope
import com.example.statstrack.R
import com.example.statstrack.helper.apiCalls.BusinessService
import com.example.statstrack.helper.apiCalls.HomePageService
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import java.util.UUID

class PhotoEditFragment(private val businessId: UUID, private val photoId: UUID) : Fragment() {

    private lateinit var deleteBtn: ImageView
    private lateinit var photo: ImageView

    private val homePageService: HomePageService by lazy {
        HomePageService(requireContext())
    }
    private val businessService: BusinessService by lazy {
        BusinessService(requireContext())
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        val view = inflater.inflate(R.layout.fragment_photo_edit, container, false)

        initPhoto()
        deleteBtn = view.findViewById(R.id.photoEditFragmentDeleteBtn)
        deleteBtn.setOnClickListener{
            lifecycleScope.launch(Dispatchers.IO) {
                businessService.deleteBusinessPhoto(photoId) { data ->
                    lifecycleScope.launch(Dispatchers.Main) {
                        if (data) {
                            parentFragmentManager.beginTransaction()
                                .hide(this@PhotoEditFragment)
                                .commit()
                        }else
                            Toast.makeText(requireContext(), "Could not delete photo", Toast.LENGTH_LONG).show()
                    }
                }
            }
        }

        return view
    }

    private fun initPhoto() {
        homePageService.getBusinessPhoto(businessId, photoId) { data ->
            lifecycleScope.launch(Dispatchers.Main) {
                if (data != null) {
                    photo.setImageBitmap(data)
                } else {
                    Toast.makeText(requireContext(), "Could not fetch data", Toast.LENGTH_LONG).show()
                }
            }
        }
    }

}