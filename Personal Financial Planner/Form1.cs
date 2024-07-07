using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Personal_Financial_Planner
{
    public partial class LoginForm : Form
    {
       
        private UserRepository userRepository;

        public LoginForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Load += LoginForm_Load;
            userRepository = new UserRepository();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            
        }

        private void btnReg_Click(object sender, EventArgs e)
        {
            var username = tbUsername.Text;
            var password = tbPassword.Text;

            if (userRepository.FindUserByUsername(username) == null)
            {
                var newUser = new User(username, password);
                
                userRepository.AddUser(newUser);
                MessageBox.Show("Успешно се регистриравте!");
            }
            else
            {
                MessageBox.Show("Корисничкото име веќе постои!");
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var username = tbUsername.Text;
            var password = tbPassword.Text;

            var user = userRepository.FindUserByUsername(username);

            if (user == null)
            {
                MessageBox.Show("Не постои корисник со тоа корисничко име.");
            }
            else if (user.Password != password)
            {
                MessageBox.Show("Погрешно корисничко име или лозинка.");
            }
            else
            {
                // Open the UserInfo form with the user data
                UserInfo userInfoForm = new UserInfo(user);
                userInfoForm.Show();
                this.Hide(); // Optionally hide the login form
            }
        }


    }


}
