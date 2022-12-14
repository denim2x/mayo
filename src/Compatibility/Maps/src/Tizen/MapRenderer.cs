using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Microsoft.Maui.Controls.Compatibility.Platform.Tizen;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Controls.Platform;
using Tizen.Location;
using Tizen.Maps;
using Pin = Microsoft.Maui.Controls.Maps.Pin;
using TPin = Tizen.Maps.Pin;

namespace Microsoft.Maui.Controls.Compatibility.Maps.Tizen
{
#pragma warning disable CS0618 // Type or member is obsolete
	public class MapRenderer : ViewRenderer<Map, MapView>
#pragma warning disable CS0618 // Type or member is obsolete
	{
		const string MoveMessageName = "MapMoveToRegion";
		const int BaseZoomLevel = 2;

		bool _disposed;
		Marker _marker;
		bool _isLocatorStarted = false;
		Lazy<Locator> _locator = new Lazy<Locator>(InitializeLocator);
		List<Pin> _pins = new List<Pin>();

		static Locator InitializeLocator()
		{
			var locator = new Locator(LocationType.Hybrid)
			{
				// Set the default interval to 15s same as UWP
				Interval = 15
			};
			return locator;
		}

		public MapRenderer()
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
		{
			if (Control == null)
			{
				var mapControl = new MapView(Forms.NativeParent, FormsMaps.MapService);

				mapControl.RenderPost += OnVisibleRegionChanged;

				SetNativeControl(mapControl);

				if (Element.LastMoveToRegion != null)
				{
					OnMoveToRegion(null, Element.LastMoveToRegion);
				}
			}

			if (e.OldElement != null)
			{
				((ObservableCollection<Pin>)e.OldElement.Pins).CollectionChanged -= OnCollectionChanged;

				foreach (Pin pin in e.OldElement.Pins)
				{
					pin.PropertyChanged -= PinOnPropertyChanged;
				}

				MessagingCenter.Unsubscribe<Map, MapSpan>(this, MoveMessageName);
			}
			if (e.NewElement != null)
			{
				((ObservableCollection<Pin>)e.NewElement.Pins).CollectionChanged += OnCollectionChanged;
				if (e.NewElement.Pins.Count > 0)
				{
					AddPins(e.NewElement.Pins);
				}

				MessagingCenter.Subscribe<Map, MapSpan>(this, MoveMessageName, OnMoveToRegion, e.NewElement);

				UpdateMapType();
				UpdateHasScrollEnabled();
				UpdateHasZoomEnabled();
				UpdateIsShowingUser();
				UpdateVisibleRegion();
				UpdateTrafficEnabled();
			}
			base.OnElementChanged(e);
		}

		protected override void Dispose(bool disposing)
		{
			if (_disposed)
			{
				return;
			}

			_disposed = true;

			if (disposing)
			{
				MessagingCenter.Unsubscribe<Map, MapSpan>(this, "MapMoveToRegion");
				if (Element != null)
				{
					((ObservableCollection<Pin>)Element.Pins).CollectionChanged -= OnCollectionChanged;
				}

				foreach (Pin pin in Element.Pins)
				{
					pin.PropertyChanged -= PinOnPropertyChanged;
				}

				Control.RenderPost -= OnVisibleRegionChanged;
				Control.Unrealize();
			}
			base.Dispose(disposing);
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == Map.MapTypeProperty.PropertyName)
				UpdateMapType();
			else if (e.PropertyName == Map.IsShowingUserProperty.PropertyName)
				UpdateIsShowingUser();
			else if (e.PropertyName == Map.HasScrollEnabledProperty.PropertyName)
				UpdateHasScrollEnabled();
			else if (e.PropertyName == Map.HasZoomEnabledProperty.PropertyName)
				UpdateHasZoomEnabled();
			else if (e.PropertyName == Map.TrafficEnabledProperty.PropertyName)
				UpdateTrafficEnabled();
		}


		void OnMoveToRegion(Map map, MapSpan span)
		{
			UpdateVisibleRegion();

			int latitudeZoomFactor = GetZoomFactor(span.LatitudeDegrees, 90.0);
			int longitudeZoomFactor = GetZoomFactor(span.LongitudeDegrees, 180.0);

			Control.Center = new Geocoordinates(span.Center.Latitude, span.Center.Longitude);
			;
			Control.ZoomLevel = BaseZoomLevel + Math.Min(latitudeZoomFactor, longitudeZoomFactor);
			UpdateVisibleRegion();
		}

		int GetZoomFactor(double degree, double targetDegree)
		{
			int factor = 0;
			double tempDegree = degree;
			while (true)
			{
				tempDegree = tempDegree * 2;
				if (tempDegree > targetDegree)
					break;
				factor++;
			}
			return factor;
		}

		void OnVisibleRegionChanged(object sender, EventArgs e)
		{
			UpdateVisibleRegion();
		}

		void UpdateVisibleRegion()
		{
			int width = Control.Geometry.Width;
			int height = Control.Geometry.Height;
			int x = Control.Geometry.X;
			int y = Control.Geometry.Y;

			Geocoordinates ul = Control.ScreenToGeolocation(new ElmSharp.Point { X = x, Y = y });
			Geocoordinates ur = Control.ScreenToGeolocation(new ElmSharp.Point { X = x + width, Y = y });
			Geocoordinates ll = Control.ScreenToGeolocation(new ElmSharp.Point { X = x, Y = y + height });
			Geocoordinates lr = Control.ScreenToGeolocation(new ElmSharp.Point { X = x + width, Y = y + height });

			double dlat = Math.Max(Math.Abs(ul.Latitude - lr.Latitude), Math.Abs(ur.Latitude - ll.Latitude));
			double dlong = Math.Max(Math.Abs(ul.Longitude - lr.Longitude), Math.Abs(ur.Longitude - ll.Longitude));

			Element.SetVisibleRegion(new MapSpan(new Position(Control.Center.Latitude, Control.Center.Longitude), dlat, dlong));
		}


		void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					AddPins(e.NewItems);
					break;
				case NotifyCollectionChangedAction.Remove:
					RemovePins(e.OldItems);
					break;
				case NotifyCollectionChangedAction.Replace:
					RemovePins(e.OldItems);
					AddPins(e.NewItems);
					break;
				case NotifyCollectionChangedAction.Reset:
					ResetPins();
					break;
				case NotifyCollectionChangedAction.Move:
					//do nothing
					break;
			}
		}

		void AddPins(IEnumerable pins)
		{
			foreach (Pin pin in pins)
			{
				pin.PropertyChanged += PinOnPropertyChanged;
				var coordinates = new Geocoordinates(pin.Position.Latitude, pin.Position.Longitude);
				var nativePin = new TPin(coordinates);
				pin.MarkerId = nativePin;
				nativePin.Clicked += (s, e) =>
				{
					pin.SendMarkerClick();
				};
				Control.Add(nativePin);
				_pins.Add(pin);
			}
		}

		void RemovePins(IEnumerable pins)
		{
			foreach (Pin pin in pins)
			{
				pin.PropertyChanged -= PinOnPropertyChanged;
				Control.Remove((TPin)pin.MarkerId);
				_pins.Remove(pin);
			}
		}

		void ResetPins()
		{
			foreach (var pin in _pins)
			{
				pin.PropertyChanged -= PinOnPropertyChanged;
			}
			_pins.Clear();
			Control.RemoveAll();
			AddPins((Element as Map).Pins);
		}

		void UpdateTrafficEnabled()
		{
			Control.TrafficEnabled = Element.TrafficEnabled;
		}

		void UpdateHasZoomEnabled()
		{
			if (Element.HasZoomEnabled == true)
				Control.TwoFingerZoomed += Dummy;
			else
				Control.TwoFingerZoomed -= Dummy;
		}

		void UpdateHasScrollEnabled()
		{
			if (Element.HasScrollEnabled == true)
				Control.Scrolled += Dummy;
			else
				Control.Scrolled -= Dummy;
		}

		void Dummy(object sender, MapGestureEventArgs e)
		{
			// The implementation of Tizen.Maps needs to be changed to remove this method
		}

		void ApplyIsShowingUser(Geocoordinates coordinates)
		{
			if (_marker == null)
			{
				_marker = new Sticker(coordinates);
				_marker.IsVisible = false;
				Control.Add(_marker);
			}
			_marker.Coordinates = coordinates;

			if (!_marker.IsVisible)
			{
				_marker.IsVisible = true;
				Control.Center = coordinates;
				Control.ZoomLevel = 13;
			}
		}

		bool IsLocatorSupported()
		{
			try
			{
				if (!LocatorHelper.IsEnabledType(LocationType.Hybrid))
				{
					Log.Error("Failed to start Locator. Please check if the Location setting on your device is On.");
					return false;
				}
			}
			catch (NotSupportedException)
			{
				Log.Error("Platform Not Supported");
				return false;
			}
			return true;
		}

		void UpdateIsShowingUser()
		{
			if (Element.IsShowingUser)
			{
				if (!IsLocatorSupported())
				{
					Element.IsShowingUser = false;
					return;
				}
				_locator.Value.LocationChanged += OnLocationChanged;
				if (!_isLocatorStarted)
				{
					_locator.Value.Start();
					_isLocatorStarted = true;
				}
			}
			else
			{
				if (_locator.IsValueCreated)
				{
					_locator.Value.LocationChanged -= OnLocationChanged;
					_locator.Value.Stop();
					_isLocatorStarted = false;
				}
				if (_marker != null)
					_marker.IsVisible = false;
			}
		}

		void OnLocationChanged(object sender, LocationChangedEventArgs e)
		{
			ApplyIsShowingUser(new Geocoordinates(e.Location.Latitude, e.Location.Longitude));
		}

		void UpdateMapType()
		{
			switch (Element.MapType)
			{
				case MapType.Street:
					Control.MapType = MapTypes.Normal;
					break;
				case MapType.Satellite:
					Control.MapType = MapTypes.Satellite;
					break;
				case MapType.Hybrid:
					Control.MapType = MapTypes.Hybrid;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		void PinOnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			Pin pin = (Pin)sender;
			var nativePin = pin.MarkerId as TPin;

			if (nativePin == null)
			{
				return;
			}

			//There is no corressponding native control propery for Label and Address.
			if (e.PropertyName == Pin.PositionProperty.PropertyName)
			{
				nativePin.Coordinates = new Geocoordinates(pin.Position.Latitude, pin.Position.Longitude);
			}
		}
	}
}
