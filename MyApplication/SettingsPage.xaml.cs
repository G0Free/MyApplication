namespace MyApplication;

public partial class SettingsPage : ContentPage
{ 
	bool switched = false;
	public SettingsPage()
	{
		InitializeComponent();
	}

    private void BackGroundToggled(object sender, ToggledEventArgs e)
    {
        Color color;

        if (switched)
		{
            color = new Color(255, 255, 255);
            switched = false;
        }
		else
		{            
            color = new Color(0, 0, 0);
            switched = true;
        }

        BackgroundColor = color;        
    }
}