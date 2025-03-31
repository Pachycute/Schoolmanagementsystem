Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports MySql.Data.MySqlClient

Public Class Signup_form

    ' Handle Login Button Click
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' Check if username or password is empty
        If String.IsNullOrWhiteSpace(txtusername.Text) OrElse String.IsNullOrWhiteSpace(txtpassword.Text) Then
            ' Display a message to the user if either username or password is empty
            MessageBox.Show("Please enter both username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return ' Exit the method or prevent further execution
        End If

        ' Create the SQL query with parameters, using BINARY for case-sensitivity
        Dim sql As String = "SELECT * FROM account_student WHERE BINARY `FULL NAME` = @username AND BINARY `PASSWORD` = @password"

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

            ' Process the data as needed
            If reader.HasRows Then
                ' Logic if data is found (login successful)
                MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                ' Logic to move to the next form
                OBJ.Show()
                Me.Hide()

            Else
                ' Logic if no data is found (invalid username/password)
                MessageBox.Show("Invalid username or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

            ' Close the reader
            reader.Close()

        Catch ex As MySqlException
            ' Handle MySQL-specific errors, such as connection issues
            Select Case ex.Number
                Case 1045 ' Access denied, incorrect username/password
                    MessageBox.Show("MySQL Error: Access denied. Please check your username or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Case 1042 ' Can't resolve host
                    MessageBox.Show("MySQL Error: Can't resolve hostname. Please check your server connection.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Case Else
                    ' Generic MySQL error message
                    MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Select

        Catch ex As Exception
            ' Handle other general errors
            MessageBox.Show("An unexpected error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            ' Ensure the connection is always closed, even if an error occurs
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        End Try

        ' Clear the textboxes after the login attempt
        txtusername.Clear()
        txtpassword.Clear()
    End Sub

    ' Handle Signup Button Click
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        ' Check if username or password is empty
        If String.IsNullOrWhiteSpace(txtusername.Text) OrElse String.IsNullOrWhiteSpace(txtpassword.Text) Then
            ' Display a message to the user if either username or password is empty
            MessageBox.Show("Please enter both username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return ' Exit the method or prevent further execution
        End If

        Try
            ' Create an SQL query to insert a new record into the account_student table
            Dim sql As String = "INSERT INTO account_student (`FULL NAME`, `PASSWORD`) VALUES (@username, @password)"
            Dim cmd As New MySqlCommand(sql, con)

            ' Add parameters to the command to prevent SQL injection
            cmd.Parameters.AddWithValue("@username", txtusername.Text)
            cmd.Parameters.AddWithValue("@password", txtpassword.Text)

            ' Open the connection
            con.Open()

            ' Execute the query
            cmd.ExecuteNonQuery()

            ' Show success message
            MessageBox.Show("Successfully added record.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Clear the textboxes
            txtusername.Clear()
            txtpassword.Clear()

            ' Optionally, show the next form or close the current form
            OBJ.Show()
            Me.Hide()

        Catch ex As MySqlException
            ' Handle MySQL-specific errors, such as connection issues
            MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Catch ex As Exception
            ' Handle other general errors
            MessageBox.Show("An unexpected error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            ' Ensure the connection is always closed, even if an error occurs
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        End Try
    End Sub

    ' Handle Show/Hide password buttons
    Private Sub Buttonhide_Click(sender As Object, e As EventArgs) Handles Buttonhide.Click
        txtpassword.UseSystemPasswordChar = True
        ' Show the show button and hide the hide button
        Buttonhide.Hide()
        Buttonshow.Show()
    End Sub

    Private Sub Buttonshow_Click(sender As Object, e As EventArgs) Handles Buttonshow.Click
        txtpassword.UseSystemPasswordChar = False
        Buttonhide.Show()
        Buttonshow.Hide()
    End Sub

    ' Handle Form Load (e.g., setting connection string)
    Private Sub Signup_form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeConnection()
    End Sub

    ' Minimize the form
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    ' Close the application
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Application.ExitThread()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Signup.Show()
        Me.Hide()
    End Sub
End Class
