///
/// Copyright (c) 2024 Vikas Reddy Thota
///

using UnityEngine;
using UnityEngine.UI;

namespace GameUIUtilities
{
    public class Scaler : MonoBehaviour
    {
        /// <summary>
        /// make true for which property you want to use for scaling
        /// Canvas Scalar is for 1080*1920 made resolution popups or canvas ui. It uses the CanvasProperty - Canvas Scalar.
        /// For better result, use CANVAS SCALAR (match with width = 0), for phones and make it (match with height = 1 for tablets)
        /// </summary>
        [SerializeField] private bool changeCanvasScaler = false;
        [SerializeField] float matchTabValue = 1f;
        [SerializeField] float matchTallDeviceValue = 0f;

        private void Awake()
        {
            ChangeCanvasSize();
        }

        void ChangeCanvasSize()
        {
            if (gameObject.TryGetComponent<CanvasScaler>(out CanvasScaler scaler))
            {
                scaler.matchWidthOrHeight = 0;
                if (changeCanvasScaler)
                {
                    if (DeviceTypeChecker.GetDeviceType() == ENUM_Device_Type.Tablet)
                    {
                        if (DeviceTypeChecker.DeviceDiagonalSizeInInches() > DeviceTypeChecker.TabletDeviceDiagonalInches)
                        {
                            if (gameObject.GetComponent<CanvasScaler>() != null) scaler.matchWidthOrHeight = matchTabValue;
                        }
                    }
                    else if (DeviceTypeChecker.GetDeviceType() != ENUM_Device_Type.Tablet && DeviceTypeChecker.DeviceDiagonalSizeInInches() > DeviceTypeChecker.PhoneDeviceDiagonalInches)
                    {
                        if (gameObject.GetComponent<CanvasScaler>() != null) scaler.matchWidthOrHeight = matchTallDeviceValue;
                    }
                }
            }
        }
    }

    public class ScreenSize
    {
        public static float GetScreenToWorldHeight
        {
            get
            {
                Vector2 topRightCorner = new Vector2(1, 1);
                Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
                var height = edgeVector.y * 2;
                return height;
            }
        }
        public static float GetScreenToWorldWidth
        {
            get
            {
                Vector2 topRightCorner = new Vector2(1, 1);
                Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
                var width = edgeVector.x * 2;
                return width;
            }
        }

    }

    public enum ENUM_Device_Type
    {
        Tablet,
        Phone,
        LowResPhone,
        UnknownDevice
    }

    public static class DeviceTypeChecker
    {
        public static bool isTablet;
        public static float PhoneDeviceDiagonalInches = 5.246f;//4.7f;
        public static float TabletDeviceDiagonalInches = 6.5f;
        public static float TallDeviceDiagonalInches = 5.8f;

        public static float DeviceDiagonalSizeInInches()
        {
            float screenWidth = Screen.width / Screen.dpi;
            float screenHeight = Screen.height / Screen.dpi;
            float diagonalInches = Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));

            return diagonalInches;
        }

        public static ENUM_Device_Type IsTablet()
        {

            float ssw;
            if (Screen.width > Screen.height) { ssw = Screen.width; } else { ssw = Screen.height; }

            if (ssw < 800) return ENUM_Device_Type.Phone;

            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                float screenWidth = Screen.width / Screen.dpi;
                float screenHeight = Screen.height / Screen.dpi;
                float size = Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));
                if (size >= 6.5f) return ENUM_Device_Type.Tablet;
                if (size <= 5f) return ENUM_Device_Type.LowResPhone;
            }

            return ENUM_Device_Type.Phone;
        }

        public static ENUM_Device_Type GetDeviceType()
        {
            float _screenWidth = Mathf.Min(Screen.width, Screen.height);
            float _screenHeight = Mathf.Max(Screen.width, Screen.height);
            float aspectRatio = _screenHeight / _screenWidth;
            bool isTablet = (DeviceDiagonalSizeInInches() > TabletDeviceDiagonalInches && aspectRatio < 2f);

            if (isTablet)
                return ENUM_Device_Type.Tablet;
            else
                return ENUM_Device_Type.Phone;
        }

        public static bool IsTabOrIpad
        {
            get
            {
                if (IsTablet() == ENUM_Device_Type.Tablet)
                {
                    if (DeviceDiagonalSizeInInches() > TabletDeviceDiagonalInches)
                    {
                        return true;
                    }
                }

                return false;
            }

        }


    }
}