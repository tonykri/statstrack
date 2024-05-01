package com.example.statstrack.fragments.homepages.searchpage

import android.Manifest
import android.annotation.SuppressLint
import android.app.AlertDialog
import android.content.Context
import android.content.Intent
import android.content.pm.PackageManager
import androidx.fragment.app.Fragment

import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.core.app.ActivityCompat
import androidx.core.content.ContextCompat
import com.example.statstrack.R
import com.example.statstrack.activities.BusinessActivity
import com.example.statstrack.fragments.homepages.SearchPageFragment
import com.example.statstrack.helper.apiCalls.dto.response.BusinessResponse
import com.google.android.gms.location.FusedLocationProviderClient
import com.google.android.gms.location.LocationServices

import com.google.android.gms.maps.CameraUpdateFactory
import com.google.android.gms.maps.GoogleMap
import com.google.android.gms.maps.OnMapReadyCallback
import com.google.android.gms.maps.SupportMapFragment
import com.google.android.gms.maps.model.LatLng
import com.google.android.gms.maps.model.MarkerOptions

class MapsFragment : Fragment(), OnMapReadyCallback {

    private lateinit var mMap: GoogleMap
    private lateinit var fusedLocationClient: FusedLocationProviderClient

    private var businesses: List<BusinessResponse> = listOf()

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val view = inflater.inflate(R.layout.fragment_maps, container, false)

        val mapFragment = childFragmentManager.findFragmentById(R.id.map) as SupportMapFragment
        mapFragment.getMapAsync(this)
        fusedLocationClient = LocationServices.getFusedLocationProviderClient(requireActivity())

        return view
    }

    override fun onMapReady(googleMap: GoogleMap) {
        mMap = googleMap

        if (ContextCompat.checkSelfPermission(requireContext(), Manifest.permission.ACCESS_FINE_LOCATION) == PackageManager.PERMISSION_GRANTED) {
            mMap.isMyLocationEnabled = true
            getLastKnownLocation()
        } else {
            ActivityCompat.requestPermissions(requireActivity(), arrayOf(Manifest.permission.ACCESS_FINE_LOCATION), REQUEST_LOCATION_PERMISSION)
        }

        mMap.setOnMarkerClickListener { marker ->
            val business = marker.tag as BusinessResponse

            val intent = Intent(context, BusinessActivity::class.java)
            intent.putExtra("business_id", business.id)
            startActivity(intent)
            true
        }

        mMap.setOnCameraMoveListener {
            sendDataAndUpdateBusinesses()
        }
    }

    @SuppressLint("MissingPermission")
    private fun getLastKnownLocation() {
        fusedLocationClient.lastLocation
            .addOnSuccessListener { location ->
                location?.let {
                    val initialLocation = LatLng(location.latitude, location.longitude)
                    mMap.moveCamera(CameraUpdateFactory.newLatLngZoom(initialLocation, DEFAULT_ZOOM))
                    sendDataAndUpdateBusinesses()
                }
            }
    }

    fun updateMap() {
        val parentFragment = parentFragment
        if(parentFragment is SearchPageFragment)
                businesses = parentFragment.getBusinesses()
        updateMarkers()
    }
    private fun sendDataAndUpdateBusinesses() {
        val projection = mMap.projection
        val visibleRegion = projection.visibleRegion

        val topRightLatLng = visibleRegion.latLngBounds.northeast
        val bottomLeftLatLng = visibleRegion.latLngBounds.southwest
        val parentFragment = parentFragment
        if(parentFragment is SearchPageFragment) {
            Log.d("LOCATION", topRightLatLng.latitude.toString())
            Log.d("LOCATION", topRightLatLng.longitude.toString())
            Log.d("LOCATION", bottomLeftLatLng.latitude.toString())
            Log.d("LOCATION", bottomLeftLatLng.longitude.toString())
            parentFragment.storeLatLng(topRightLatLng, bottomLeftLatLng) {
                businesses = parentFragment.getBusinesses()
            }
        }
        updateMarkers()
    }

    private fun updateMarkers() {
        mMap.clear()

        for (business in businesses) {
            val location = LatLng(business.latitude, business.longitude)
            val marker = mMap.addMarker(MarkerOptions().position(location).title(business.brand))

            marker?.tag = business
        }
    }

    @Deprecated("Deprecated in Java")
    override fun onRequestPermissionsResult(requestCode: Int, permissions: Array<out String>, grantResults: IntArray) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults)
        when (requestCode) {
            REQUEST_LOCATION_PERMISSION -> {
                if (grantResults.isNotEmpty() && grantResults[0] == PackageManager.PERMISSION_GRANTED) {
                    getLastKnownLocation()
                } else {
                    showMessage(requireContext(), "Necessary Permission", "In order to provide you our services you need to accept the location permission")
                }
            }
        }
    }

    private fun showMessage(context: Context, title: String, message: String) {
        val builder = AlertDialog.Builder(context)
        builder.setTitle(title)
        builder.setMessage(message)
        builder.setPositiveButton("OK") { dialog, _ ->
            dialog.dismiss()
            ActivityCompat.requestPermissions(requireActivity(), arrayOf(Manifest.permission.ACCESS_FINE_LOCATION), REQUEST_LOCATION_PERMISSION)
        }
        val dialog = builder.create()
        dialog.show()
    }


    companion object {
        private const val DEFAULT_ZOOM = 17f
        private const val REQUEST_LOCATION_PERMISSION = 1
    }
}