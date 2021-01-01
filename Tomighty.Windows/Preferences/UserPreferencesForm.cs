﻿//
//  Tomighty - http://www.tomighty.org
//
//  This software is licensed under the Apache License Version 2.0:
//  http://www.apache.org/licenses/LICENSE-2.0.txt
//

namespace Tomighty.Windows.Preferences
{
    public partial class UserPreferencesForm : BaseWindow
    {
        private IUserPreferences userPreferences;

        public UserPreferencesForm(IUserPreferences userPreferences)
        {
            this.userPreferences = userPreferences;

            InitializeComponent();

            pomodoroDurationTextBox.Value = userPreferences.GetIntervalDuration(IntervalType.Pomodoro).Minutes;
            shortBreakDurationTextBox.Value = userPreferences.GetIntervalDuration(IntervalType.ShortBreak).Minutes;
            longBreakDurationTextBox.Value = userPreferences.GetIntervalDuration(IntervalType.LongBreak).Minutes;
            maxPomodoroCountTextBox.Value = userPreferences.MaxPomodoroCount;
            toastNotification.Checked = userPreferences.ShowToastNotifications;
            soundNotifications.Checked = userPreferences.PlaySoundNotifications;
            autoupdate.Checked = userPreferences.AutoUpdate;
        }

        private void OnCancelButtonClick(object sender, System.EventArgs e)
        {
            Close();
        }

        private void OnOkButtonClick(object sender, System.EventArgs e)
        {
            userPreferences.Update(newPreferences =>
            {
                newPreferences.SetIntervalDuration(IntervalType.Pomodoro, Duration.InMinutes((int)pomodoroDurationTextBox.Value));
                newPreferences.SetIntervalDuration(IntervalType.ShortBreak, Duration.InMinutes((int)shortBreakDurationTextBox.Value));
                newPreferences.SetIntervalDuration(IntervalType.LongBreak, Duration.InMinutes((int)longBreakDurationTextBox.Value));
                newPreferences.MaxPomodoroCount = (int)maxPomodoroCountTextBox.Value;
                newPreferences.ShowToastNotifications = toastNotification.Checked;
                newPreferences.PlaySoundNotifications = soundNotifications.Checked;
                newPreferences.AutoUpdate = autoupdate.Checked;
            });

            Close();
        }
    }
}
