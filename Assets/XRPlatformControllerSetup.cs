using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.XR.Management;
#else
using UnityEngine.XR.Management;
#endif

namespace Unity.Template.VR
{
    internal class XRPlatformControllerSetup : MonoBehaviour
    {
        [SerializeField]
        GameObject m_LeftController;

        [SerializeField]
        GameObject m_RightController;

        [SerializeField]
        GameObject m_LeftControllerOculusPackage;

        [SerializeField]
        GameObject m_RightControllerOculusPackage;

        public XRNode xr_input_left;
        public XRNode xr_input_right;

        GameObject marker_hfov_left_modifier = null;
        GameObject marker_hfov_right_modifier = null;
        GameObject marker_vfov_top_modifier = null;
        GameObject marker_vfov_bottom_modifier = null;
        
        public TMP_Text text_fov;
        public TMP_Text text_fov2;
        float hfov = 60;
        float vfov = 60;
        float vfov_bottom = 0;

        void Start()
        {
#if UNITY_EDITOR
            var loaders = XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(BuildTargetGroup.Standalone).Manager.activeLoaders;
#else
            var loaders = XRGeneralSettings.Instance.Manager.activeLoaders;
#endif

            foreach (var loader in loaders)
            {
                if (loader.name.Equals("Oculus Loader"))
                {
                    m_RightController.SetActive(false);
                    m_LeftController.SetActive(false);
                    m_RightControllerOculusPackage.SetActive(true);
                    m_LeftControllerOculusPackage.SetActive(true);
                }
            }

            // The markers are parented to empty objects. These parents are at the position of the camera/headset and move with it (by being parented as well)
            // while the actual markers are far away in the distance.
            // To move these far away markers around the headset I just rotate these empty parents which I called modifier to not have to deal with the sine
            // and cosine stuff manually. Also this allows me to change the relative position in the editor.
            marker_hfov_left_modifier = GameObject.Find("marker_hfov_left_modifier");
            marker_hfov_right_modifier = GameObject.Find("marker_hfov_right_modifier");
            marker_vfov_top_modifier = GameObject.Find("marker_vfov_top_modifier");
            marker_vfov_bottom_modifier = GameObject.Find("marker_vfov_bottom_modifier");

            GameObject marker_h = GameObject.Find("marker_h");  // Use the marker_h as a template to create text markers and then destroy this template object
            GameObject marker_v = GameObject.Find("marker_v");
            for (int i = 10; i < 180; i += 10)
            {
                GameObject h_a = GameObject.Instantiate(marker_h, marker_h.transform.parent);
                GameObject h_b = GameObject.Instantiate(marker_h, marker_h.transform.parent);
                GameObject v_a = GameObject.Instantiate(marker_v, marker_v.transform.parent);
                GameObject v_b = GameObject.Instantiate(marker_v, marker_v.transform.parent);
                h_a.transform.localPosition = new Vector3(0, 0, 0);
                h_b.transform.localPosition = new Vector3(0, 0, 0);
                v_a.transform.localPosition = new Vector3(0, 0, 0);
                v_b.transform.localPosition = new Vector3(0, 0, 0);
                h_a.transform.Rotate(0.0f, i, 0.0f, Space.Self);
                h_b.transform.Rotate(0.0f,-i, 0.0f, Space.Self);
                v_a.transform.Rotate( i,0.0f, 0.0f, Space.Self);
                v_b.transform.Rotate(-i,0.0f, 0.0f, Space.Self);
                h_a.transform.Translate(0.0f, -90.0f, 500.0f, Space.Self);
                h_b.transform.Translate(0.0f, -90.0f, 500.0f, Space.Self);
                v_a.transform.Translate(90.0f, 0.0f, 500.0f, Space.Self);
                v_b.transform.Translate(90.0f, 0.0f, 500.0f, Space.Self);
                h_a.GetComponent<TMP_Text>().text = i.ToString();
                h_b.GetComponent<TMP_Text>().text = i.ToString();
                v_a.GetComponent<TMP_Text>().text = i.ToString();
                v_b.GetComponent<TMP_Text>().text = i.ToString();
                if(i==10)   // destroy the vertical marker that would overlap a horizontal marker at the same position
                    GameObject.Destroy(v_a);
            }
            GameObject.Destroy(marker_h);
            GameObject.Destroy(marker_v);
        }

        void Update()
        {
            InputDevice input_device_left  = InputDevices.GetDeviceAtXRNode(xr_input_left);
            InputDevice input_device_right = InputDevices.GetDeviceAtXRNode(xr_input_right);
            Vector2 input_axis_left;
            Vector2 input_axis_right;
            bool input_button_left_primary;
            bool input_button_right_primary;
            bool input_left_trigger;
            bool input_right_trigger;
            bool input_button_left_secondary;
            bool input_button_right_secondary;
            bool input_left_grip;
            bool input_right_grip;
            bool input_left_joystick;
            bool input_right_joystick;
            input_device_left .TryGetFeatureValue(CommonUsages.primary2DAxis, out input_axis_left);
            input_device_right.TryGetFeatureValue(CommonUsages.primary2DAxis, out input_axis_right);
            input_device_left .TryGetFeatureValue(CommonUsages.primaryButton, out input_button_left_primary);
            input_device_right.TryGetFeatureValue(CommonUsages.primaryButton, out input_button_right_primary);
            input_device_left .TryGetFeatureValue(CommonUsages.triggerButton, out input_left_trigger);
            input_device_right.TryGetFeatureValue(CommonUsages.triggerButton, out input_right_trigger);
            input_device_left .TryGetFeatureValue(CommonUsages.secondaryButton, out input_button_left_secondary);
            input_device_right.TryGetFeatureValue(CommonUsages.secondaryButton, out input_button_right_secondary);
            input_device_left .TryGetFeatureValue(CommonUsages.gripButton, out input_left_grip);
            input_device_right.TryGetFeatureValue(CommonUsages.gripButton, out input_right_grip);
            input_device_left .TryGetFeatureValue(CommonUsages.primary2DAxisClick, out input_left_joystick);
            input_device_right.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out input_right_joystick);

            Vector3 keyboard_axis = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Vertical_Decoupled"));
            keyboard_axis.Normalize();

            // unify the input of both thumbsticks/touchpads to one vector. This also extends the value range to -2.0 - 2.0 if both thumbsticks are pushed in the same direction but that doesn't matter.
            Vector2 input_axis = input_axis_left + input_axis_right + new Vector2(keyboard_axis.x,keyboard_axis.y);
            bool input_button_trigger = input_left_trigger || input_right_trigger || keyboard_axis.z>0;
            bool input_button_grip = input_left_grip || input_right_grip || keyboard_axis.z < 0;
            bool input_button_face = input_button_left_primary || input_button_right_primary || input_button_left_secondary || input_button_right_secondary || Input.GetButton("Jump");
            bool input_button_joystick = input_left_joystick || input_right_joystick || Input.GetButton("Fire1");

            marker_hfov_left_modifier  .transform.localRotation = Quaternion.identity;    // reset the rotation
            marker_hfov_right_modifier .transform.localRotation = Quaternion.identity;
            marker_vfov_top_modifier   .transform.localRotation = Quaternion.identity;
            marker_vfov_bottom_modifier.transform.localRotation = Quaternion.identity;

            if (Math.Abs(input_axis.x) < 0.5f)  // dead zone of 50 % in the center to avoid unintentional movement, especially when only wanting to change one axis
                input_axis.x = 0;
            if (Math.Abs(input_axis.y) < 0.5f)
                input_axis.y = 0;
            if (input_axis.x < 0)               // all values are now at least 0.5 or -0.5 if negative, move towards 0 so that the total value range is -0.5 to 0.5 to allow very slow movements as well
                input_axis.x += 0.5f;
            if (input_axis.y < 0)
                input_axis.y += 0.5f;
            if (input_axis.x > 0)
                input_axis.x -= 0.5f;
            if (input_axis.y > 0)
                input_axis.y -= 0.5f;

            if (input_button_joystick)  // reset asymetric vfov with thumbstick/touchpad click
                vfov_bottom = 0;

            // hfov and vfov are in total fov (left+right / top+bottom) while vfov_top is only top
            hfov += 30.0f * Time.deltaTime * input_axis.x;  // change the set FOV in an FPS independent way
            vfov += 30.0f * Time.deltaTime * input_axis.y;
            if(vfov_bottom!=0) // if an asymetric vfov has been set, change the top offset as well so that the two vertical bars can be moved independently
                vfov_bottom -= 15.0f * Time.deltaTime * input_axis.y;
            vfov_bottom -= 5.0f * Time.deltaTime * ((input_button_trigger?1:0)-(input_button_grip?1:0));
            hfov = Math.Clamp(0,hfov,360);
            vfov = Math.Clamp(0,vfov,360);
            vfov_bottom = Math.Clamp(-50, vfov_bottom, 50);

            // Make the markers blink while any face button is pressed
            if (input_button_face && ((int)(Time.realtimeSinceStartupAsDouble * 10) % 2)==1)
            {
                marker_hfov_left_modifier  .SetActive(false);
                marker_hfov_right_modifier .SetActive(false);
                marker_vfov_top_modifier   .SetActive(false);
                marker_vfov_bottom_modifier.SetActive(false);
            }
            else
            {
                marker_hfov_left_modifier  .SetActive(true);
                marker_hfov_right_modifier .SetActive(true);
                marker_vfov_top_modifier   .SetActive(true);
                marker_vfov_bottom_modifier.SetActive(true);
            }

            int hfov_int = (int)Math.Round(hfov)/2*2;    // two degree steps so that each side moves in whole degrees
            int vfov_top_int = (int)Math.Round(vfov/2);
            int vfov_bottom_int = (int)Math.Round(vfov/2+vfov_bottom);
            // rotate the markers to show the currently set FOV rounded to integers
            marker_hfov_left_modifier  .transform.Rotate(0.0f,-(float)hfov_int/2, 0.0f, Space.Self);
            marker_hfov_right_modifier .transform.Rotate(0.0f, (float)hfov_int/2, 0.0f, Space.Self);
            marker_vfov_top_modifier   .transform.Rotate(-(float)vfov_top_int   , 0.0f, 0.0f, Space.Self);
            marker_vfov_bottom_modifier.transform.Rotate( (float)vfov_bottom_int, 0.0f, 0.0f, Space.Self);

            FindObjectsOfType<Camera>();

            text_fov.text = "HFOV: " + hfov_int + "\n";
            // split the fov display when an asymetric vfov was set
            if (vfov_top_int==vfov_bottom_int)
                text_fov.text += "VFOV: " + (vfov_top_int+vfov_bottom_int) + "\n";
            else
                text_fov.text += "VFOV: " + vfov_top_int + "+" + vfov_bottom_int + "=" + (vfov_top_int+vfov_bottom_int) + "\n";
            text_fov.text += "Rendered HFOV: " + (int)Math.Round(Camera.VerticalToHorizontalFieldOfView(Camera.main.fieldOfView,Camera.main.aspect)) + "\n" +
                             "Rendered VFOV: " + (int)Camera.main.fieldOfView;

            text_fov2.text = text_fov.text; // copy text to the display on the other side
        }
    }
}
