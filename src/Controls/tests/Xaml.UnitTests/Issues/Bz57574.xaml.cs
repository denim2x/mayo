using System;
using Microsoft.Maui.Controls.Core.UnitTests;
using NUnit.Framework;

namespace Microsoft.Maui.Controls.Xaml.UnitTests
{
	public class Bz57574NotificationEventArgs<T> : EventArgs
	{
		public T Message { get; set; }
	}

	public class Bz57574Notificator<T> : View
	{
		public void Notify(T message)
		{
			Notified?.Invoke(this, new Bz57574NotificationEventArgs<T> { Message = message });
		}

		public event EventHandler<Bz57574NotificationEventArgs<T>> Notified;
	}

	public partial class Bz57574
	{
		public Bz57574()
		{
			notificator.Notified += OnNotified;
			InitializeComponent();
		}

		public Bz57574(bool useCompiledXaml)
		{
			//this stub will be replaced at compile time
		}


		string notification;
		public void OnNotified(object sender, Bz57574NotificationEventArgs<string> args)
		{
			notification = args.Message;
		}

		[TestFixture]
		class Tests
		{
			[TearDown]
			public void TearDown()
			{
				Application.Current = null;
			}

			[TestCase(true)]
			[TestCase(false)]
			public void EventWithGenericEventHandlers(bool useCompiledXaml)
			{
				var layout = new Bz57574(useCompiledXaml);
				Assume.That(layout.notification, Is.Null);
				layout.notificator.Notify("Foo");
				Assert.That(layout.notification, Is.EqualTo("Foo"));
			}
		}
	}
}