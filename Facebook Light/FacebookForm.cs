using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using FacebookWrapper.ObjectModel;



namespace C20_Ex20_Tomer_204389605_Binyamin_308051903
{
    public partial class FacebookForm : Form
    {
        private FacebookManager m_FacebookManager;
        public string NewPostMsg { get; set; }
        private readonly object sr_FinderFeatureLocker = new object();
        public FinderFeature m_FinderFeature { get; set; }
        public FacebookForm()
        {
            InitializeComponent();
            m_FacebookManager = FacebookManager.GetInstance();

            if (m_FacebookManager.AppSettingsInstance.RememberUser)
            {
                m_FacebookManager.AppSettingsInstance = AppSettings.GetInstance();
                this.Location = m_FacebookManager.AppSettingsInstance.LastWindowLocation;
                this.m_CheckboxRemberMe.Checked = m_FacebookManager.AppSettingsInstance.RememberUser;
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            try
            {
                if (m_FacebookManager.AppSettingsInstance.RememberUser &&
                    !string.IsNullOrEmpty(m_FacebookManager.AppSettingsInstance.AccessToken))
                {

                    panel1.Visible = true;
                    m_PicBoxProfilePic.Visible = true;
                    login();
                    m_LoginLogoutBtn.Text = "Log Out";
                    m_CheckboxRemberMe.Enabled = true;
                    //Enable Sort
                    sortFriendsByFirstNameBtn.Enabled = true;
                    sortFriendsByLastNameBtn.Enabled = true;
                    //Enable Post Btn
                    m_NewPostBtn.Enabled = true;
                    m_NewPostBtn.BackColor = Color.CornflowerBlue;
                    //Enable Run Hour Test Btn
                    commentsBtn.Enabled = true;
                    LikesBtn.Enabled = true;
                    LikesBtn.BackColor = Color.CornflowerBlue;
                    commentsBtn.BackColor = Color.CornflowerBlue;
                    //Enable Run Abroad Friends Btn
                    m_RunAbroadFriendsBtn.Enabled = true;
                    m_RunAbroadFriendsBtn.BackColor = Color.CornflowerBlue;
                    //Enalbe Post TextBox
                    m_TextBoxNewPost.BackColor = Color.PaleTurquoise;
                    m_TextBoxNewPost.Enabled = true;
                    //Enable Radio Btn on Abroad Test
                    m_RadioBtnMaleFriends.Enabled = true;
                    m_RadioBtnFemaleFriends.Enabled = true;
                    //Hide please LogIn label
                    m_PleaseLogInLabel.Visible = false;

                    login();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            m_FacebookManager.AppSettingsInstance.RememberUser = this.m_CheckboxRemberMe.Checked;
            m_FacebookManager.AppSettingsInstance.LastWindowLocation = this.Location;
            m_FacebookManager.AppSettingsInstance.LastWindowsSize = this.Size;
            if (m_FacebookManager.LoginResult != null)
            {
                if (!string.IsNullOrEmpty(m_FacebookManager.LoginResult.AccessToken))
                {
                    m_FacebookManager.AppSettingsInstance.AccessToken = m_FacebookManager.LoginResult.AccessToken;
                }
            }

            m_FacebookManager.AppSettingsInstance.SaveToFile();
        }

        private void loginLogoutBtn_Click(object sender, EventArgs e)
        {
            if (m_LoginLogoutBtn.Text == "Log In")
            {
                panel1.Visible = true;
                m_PicBoxProfilePic.Visible = true;
                login();
                m_LoginLogoutBtn.Text = "Log Out";
                m_CheckboxRemberMe.Enabled = true;
                //Enable Sort
                sortFriendsByFirstNameBtn.Enabled = true;
                sortFriendsByLastNameBtn.Enabled = true;
                //Enable Post Btn
                m_NewPostBtn.Enabled = true;
                m_NewPostBtn.BackColor = Color.CornflowerBlue;
                //Enable Run Hour Test Btn
                commentsBtn.Enabled = true;
                LikesBtn.Enabled = true;
                LikesBtn.BackColor = Color.CornflowerBlue;
                commentsBtn.BackColor = Color.CornflowerBlue;
                //Enable Run Abroad Friends Btn
                m_RunAbroadFriendsBtn.Enabled = true;
                m_RunAbroadFriendsBtn.BackColor = Color.CornflowerBlue;
                //Enalbe Post TextBox
                m_TextBoxNewPost.BackColor = Color.PaleTurquoise;
                m_TextBoxNewPost.Enabled = true;
                //Enable Radio Btn on Abroad Test
                m_RadioBtnMaleFriends.Enabled = true;
                m_RadioBtnFemaleFriends.Enabled = true;
                //Hide please LogIn label
                m_PleaseLogInLabel.Visible = false;
            }
            else
            {
                panel1.Visible = false;
                messageTextBox.Text = string.Empty;
                m_CheckboxRemberMe.Visible = true;
                m_PicBoxProfilePic.Visible = false;
                logout();
                m_CheckboxRemberMe.Enabled = true;
                m_CheckboxRemberMe.Checked = false;
                //Disable Sort
                sortFriendsByFirstNameBtn.Enabled = false;
                sortFriendsByLastNameBtn.Enabled = false;
                //Disable Post Btn 
                m_NewPostBtn.Enabled = false;
                m_NewPostBtn.BackColor = Color.LightCoral;
                //Disable Run Hour Test Btn
                commentsBtn.Enabled = false;
                LikesBtn.Enabled = false;
                LikesBtn.BackColor = Color.LightCoral;
                commentsBtn.BackColor = Color.LightCoral;
                //Disable Run Abroad Friends Btn
                m_RunAbroadFriendsBtn.Enabled = false;
                m_RunAbroadFriendsBtn.BackColor = Color.LightCoral;
                //Disable Post TextBox
                m_TextBoxNewPost.BackColor = Color.DarkGray;
                m_TextBoxNewPost.Clear();
                m_TextBoxNewPost.Enabled = false;
                //Disable Radio Btn on Abroad Test
                m_RadioBtnMaleFriends.Enabled = false;
                m_RadioBtnFemaleFriends.Enabled = false;
                //Show please LogIn label
                m_PleaseLogInLabel.Visible = true;
                cleanUserData();
                m_LoginLogoutBtn.Text = "Log In";
            }
        }

        private void cleanUserData()
        {
            m_ListBoxAllPost.Items.Clear();
            m_ListBoxAllFriends.Items.Clear();
            m_ListBoxAllCheckIns.Items.Clear();
            m_PicBoxProfilePic.Image = null;
        }

        private void login()
        {
            m_FacebookManager.Login();
            m_CheckboxRemberMe.Enabled = true;
            m_LoginLogoutBtn.Text = "Logout";
            this.fetchUserInfo();
        }

        private void fetchUserInfo()
        {
            try
            {
                m_PicBoxProfilePic.LoadAsync(m_FacebookManager.LoggedInUser.PictureNormalURL);
                this.fetchUserPosts();
                this.fetchUserFriends();
                this.fetchUserCheckins();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fetchUserFriends()
        {
            m_ListBoxAllFriends.DataSource = friendListBindingSource;
            m_ListBoxAllFriends.DisplayMember = "Name";
            friendListBindingSource.DataSource = m_FacebookManager.LoggedInUserFriends;
        }

        private void fetchUserFriendsGender()
        {
            List<string> userFriendsMale = m_FacebookManager.FetchUserFriendsMale();
            List<string> userFriendsFemale = m_FacebookManager.FetchUserFriendsFemale();

            if (m_RadioBtnMaleFriends.Checked)
            {
                m_PictureBoxMale.Visible = true;
                m_PictureBoxFemale.Visible = false;
                m_NoGenderSelectedLabel.Visible = false;
                m_ListBoxFriendsByGender.Items.Clear();

                foreach (string MaleGender in userFriendsMale)
                {
                    m_ListBoxFriendsByGender.Items.Add(MaleGender);
                }

                if (userFriendsMale.Count == 0)
                {
                    m_ListBoxFriendsByGender.Items.Add("No male friends to retrieve :(");
                }
            }

            else if(m_RadioBtnFemaleFriends.Checked)
            {
                m_PictureBoxMale.Visible = false;
                m_PictureBoxFemale.Visible = true;
                m_NoGenderSelectedLabel.Visible = false;
                m_ListBoxFriendsByGender.Items.Clear();

                foreach (string FemaleGender in userFriendsFemale)
                {
                    m_ListBoxFriendsByGender.Items.Add(FemaleGender);
                }

                if (userFriendsFemale.Count == 0)
                {
                    m_ListBoxFriendsByGender.Items.Add("No female friends to retrieve :(");
                }
            }
            else
            {
                m_NoGenderSelectedLabel.Visible = true;
                m_PictureBoxMale.Visible = false;
                m_PictureBoxFemale.Visible = false;
            }          
        }

        private void fetchUserCheckins()
        {
            checkinBindingSource.DataSource = m_FacebookManager.LoggedInUser.Checkins;
        }

        private void fetchUserPosts()
        {
            postBindingSource.DataSource = m_FacebookManager.LoggedInUser.Posts;
        }

        private void m_NewPostBtn_Click(object sender, EventArgs e)
        {
            NewPostMsg = m_TextBoxNewPost.Text;

            if (!string.IsNullOrEmpty(NewPostMsg))
            {
                try
                {
                    m_FacebookManager.LoggedInUser.PostStatus(NewPostMsg);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Write something to post");
            }
        }

        private void logout()
        {
            m_FacebookManager.Logout();
            m_CheckboxRemberMe.Enabled = false;
            m_CheckboxRemberMe.Checked = false;
            cleanUI();
            m_LoginLogoutBtn.Text = "Login";
        }

        private void cleanUI()
        {
            m_ListBoxAllCheckIns.DataSource = null;
            m_ListBoxAllFriends.DataSource = null;
            m_ListBoxAllPost.DataSource = null;
            m_PicBoxProfilePic.Image = null;
        }

        private void m_RunFriendsByGenderBtn_Click(object sender, EventArgs e)
        {
            this.fetchUserFriendsGender();
        }

        private void m_BestTimeForPost_Click(object sender, EventArgs e)
        {

        }

        private void clearFriendsListBox()
        {
            m_ListBoxAllFriends.DataSource = null;
        }

        private void sortFriendsByFirstNameBtn_Click(object sender, EventArgs e)
        {
            sortFriendsByFirstNameBtn.BackColor = Color.LimeGreen;
            sortFriendsByLastNameBtn.BackColor = Color.IndianRed;
            clearFriendsListBox();
            m_FacebookManager.SortFriendByFirstName();
            fetchUserFriends();
        }

        private void sortFriendsByLastNameBtn_Click(object sender, EventArgs e)
        {
            sortFriendsByFirstNameBtn.BackColor = Color.IndianRed;
            sortFriendsByLastNameBtn.BackColor = Color.LimeGreen;
            clearFriendsListBox();
            m_FacebookManager.SortFriendByLastName();
            fetchUserFriends();
        }

        private void LikesBtn_Click(object sender, EventArgs e)
        {
            this.LikesBtn.BackColor = Color.Yellow;
            this.commentsBtn.BackColor = Color.CornflowerBlue;
            this.m_CalculatinBestTime.Visible = true;
            m_bestTimeForNewPostlabel.Visible = false;
            new Thread(FinderByLikes).Start();
        }
        private void FinderByLikes()
        {
            lock (sr_FinderFeatureLocker)
            {
                try
                {
                    m_FacebookManager.startFindByLikes();
                    int time = m_FacebookManager.FinderFeature.IStrategyFinder.Time;
                    string theBestPost = m_FacebookManager.FinderFeature.IStrategyFinder.BestPost;
                    m_bestTimeForNewPostlabel.Invoke(new Action(() => m_bestTimeForNewPostlabel.Visible = false));
                    m_bestTimeForNewPostlabel.Invoke(new Action(() => m_bestTimeForNewPostlabel.Text = "The best time for a new post is at  "));
                    m_bestTimeForNewPostlabel.Invoke(new Action(() => m_bestTimeForNewPostlabel.Text += time.ToString()));
                    m_bestTimeForNewPostlabel.Invoke(new Action(() => m_bestTimeForNewPostlabel.Visible = true));
                    m_bestTimeForNewPostlabel.Invoke(new Action(() => m_bestTimeForNewPostlabel.Text += ":00"));
                    LikesBtn.Invoke(new Action(() => LikesBtn.BackColor = Color.PaleGreen));
                    m_CalculatinBestTime.Invoke(new Action(() => m_CalculatinBestTime.Visible = false));
                    m_PicBoxThumbnUp.Invoke(new Action(() => m_PicBoxThumbnUp.Visible = true));
                    m_PicBoxThumbnDown.Invoke(new Action(() => m_PicBoxThumbnDown.Visible = false));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void commentsBtn_Click(object sender, EventArgs e)
        {
            this.LikesBtn.BackColor = Color.CornflowerBlue;
            this.commentsBtn.BackColor = Color.Yellow;

            this.m_CalculatinBestTime.Visible = true;
            m_bestTimeForNewPostlabel.Visible = false;
            new Thread(FinderByComments).Start();
        }
        private void FinderByComments()
        {
            lock (sr_FinderFeatureLocker)
            {
                try
                {
                    List<string> posts = new List<string>();
                    m_FacebookManager.startFindByComments();
                    int time = m_FacebookManager.FinderFeature.IStrategyFinder.Time;
                    string theBestPost = m_FacebookManager.FinderFeature.IStrategyFinder.BestPost;
                    m_bestTimeForNewPostlabel.Invoke(new Action(() => m_bestTimeForNewPostlabel.Text = "The best time for a new post is at  "));
                    m_bestTimeForNewPostlabel.Invoke(new Action(() => m_bestTimeForNewPostlabel.Text += time.ToString()));
                    m_bestTimeForNewPostlabel.Invoke(new Action(() => m_bestTimeForNewPostlabel.Visible = true));
                    m_bestTimeForNewPostlabel.Invoke(new Action(() => m_bestTimeForNewPostlabel.Text += ":00"));

                    m_bestTimeForNewPostlabel.Invoke(new Action(() => textOfBestPost.Visible = true));

                    textOfBestPost.Invoke(new Action(() => textOfBestPost.Text = theBestPost.ToString()));


                    commentsBtn.Invoke(new Action(() => commentsBtn.BackColor = Color.PaleGreen));
                    m_CalculatinBestTime.Invoke(new Action(() => m_CalculatinBestTime.Visible = false));
                    m_PicBoxThumbnUp.Invoke(new Action(() => m_PicBoxThumbnUp.Visible = true));
                    m_PicBoxThumbnDown.Invoke(new Action(() => m_PicBoxThumbnDown.Visible = false));

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void textOfBestPost_Click(object sender, EventArgs e)
        {

        }
    }
}
