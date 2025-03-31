Imports MySql.Data.MySqlClient

Public Class AdminSignup

    ' Connection object shared across methods


    ' Handle Sign Up Button Click
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        ' Check if username or password is empty
        If String.IsNullOrWhiteSpace(txtusername.Text) OrElse String.IsNullOrWhiteSpace(txtpassword.Text) Then
            MessageBox.Show("Please enter both username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return ' Exit the method or prevent further execution
        End If

        ' Prepare the SQL query to insert data
        Dim sql As String = "INSERT INTO account_admin (`NAME`,`PASSWORD`) VALUES (@username, @password)"

        ' Create a MySQL command object
        Dim cmd As New MySqlCommand(sql, con)

        ' Add parameters to prevent SQL injection
        cmd.Parameters.AddWithValue("@username", txtusername.Text)
        cmd.Parameters.AddWithValue("@password", txtpassword.Text)

        Try
            ' Open the connection
            con.Open()

            ' Execute the query
            cmd.ExecuteNonQuery()

            ' Show success message
            MsgBox("Successfully added record")

            ' Transition to next form and clear textboxes
            OBJ1.Show()
            Me.Hide()
            txtusername.Clear()
            txtpassword.Clear()

        Catch ex As MySqlException
            ' Handle MySQL-specific errors
            MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Catch ex As Exception
            ' Handle other general errors
            MessageBox.Show("An unexpected error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            ' Ensure the connection is always closed
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        End Try
    End Sub

    ' Handle Login Button Click
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' Check if username or password is empty
        If String.IsNullOrWhiteSpace(txtusername.Text) OrElse String.IsNullOrWhiteSpace(txtpassword.Text) Then
            MessageBox.Show("Please enter both username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return ' Exit the method or prevent further execution
        End If

        ' Prepare the SQL query with parameters
        Dim sql As String = "SELECT * FROM account_admin WHERE BINARY `NAME` = @username AND BINARY `PASSWORD` = @password"

        ' Create a MySQL command object
        Dim cmd As New MySqlCommand(sql, con)

        ' Add parameters to the command to prevent SQL injection
        cmd.Parameters.AddWithValue("@username", txtusername.Text)
        cmd.Parameters.AddWithValue("@password", txtpassword.Text)

        Try
            ' Open the connection
            con.Open()

            ' Execute the query
            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            ' Check if the login was successful
            If reader.HasRows Then
                MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                ' Logic to move to the next form
                OBJ1.Show()
                Me.Hide()
            Else
                MessageBox.Show("Invalid username or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

            reader.Close()

        Catch ex As MySqlException
            ' Handle MySQL-specific errors
            MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Catch ex As Exception
            ' Handle general errors
            MessageBox.Show("An unexpected error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            ' Ensure the connection is always closed
            If con.State = ConnectionState.Open Then
                con.Close()
            End If

            ' Clear the textboxes after the login attempt
            txtusername.Clear()
            txtpassword.Clear()
        End Try
    End Sub

    ' Handle password visibility toggle (show password)
    Private Sub Buttonshow_Click(sender As Object, e As EventArgs) Handles Buttonshow.Click
        txtpassword.UseSystemPasswordChar = False
        Buttonhide.Show()
        Buttonshow.Hide()
    End Sub

    ' Handle password visibility toggle (hide password)
    Private Sub Buttonhide_Click(sender As Object, e As EventArgs) Handles Buttonhide.Click
        txtpassword.UseSystemPasswordChar = True
        Buttonhide.Hide()
        Buttonshow.Show()
    End Sub

    ' Handle form transition to Signup
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Signup.Show()
        Me.Hide()
    End Sub

    ' Minimize the form
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    ' Exit the application
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Application.ExitThread()
    End Sub

    Private Sub AdminSignup_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeConnection()
    End Sub
End Class
