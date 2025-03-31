Imports MySql.Data.MySqlClient

Module MYSQL
    ' Retaining all your original variables
    Public OBJ1 As New Admin
    Public OBJ As New Home_Page_Students
    Public con As MySqlConnection
    Public ds As New DataSet
    Public cmd As MySqlCommand = New MySqlCommand
    Public dr As MySqlDataReader
    Public adapter As MySqlDataAdapter
    Public Table As DataTable
    Public sql As String
    Public data As New DataSet
    Public da As MySqlDataAdapter

    ' Global flag to prevent form closure
    Public IsDatabaseInitializing As Boolean = False

    ' Function to prompt user for database name and password, and set up the connection
    Public Sub InitializeConnection()
        Dim databaseName As String
        Dim password As String

        ' Set the flag to prevent closing
        IsDatabaseInitializing = True

        ' Loop to ask for database name and password until successful connection
        Do
            databaseName = InputBox("Please enter the database name:", "Database Name")

            ' Check if the database name is empty
            If String.IsNullOrWhiteSpace(databaseName) Then
                MessageBox.Show("A database name is required to connect to the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Continue Do ' Keep asking for the name again
            End If

            ' Prompt for database password
            password = InputBox("Please enter the database password:", "Database Password")

            ' Check if the password is empty
            If String.IsNullOrWhiteSpace(password) Then
                MessageBox.Show("A password is required to connect to the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Continue Do ' Keep asking for the password again
            End If

            ' Initialize the connection string with the user-provided database name and password
            Try
                con = New MySqlConnection($"Server=127.0.0.1;Database={databaseName};Uid=root;Pwd={password};")

                ' Test the connection to validate the database name and password
                con.Open()
                MessageBox.Show("Connection successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Do ' Exit the loop if the connection is successful
            Catch ex As MySqlException
                ' Handle specific MySQL exceptions
                If ex.Number = 1049 Then ' Error code for unknown database
                    MessageBox.Show("The specified database does not exist. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                ElseIf ex.Number = 1045 Then ' Access denied
                    MessageBox.Show("Incorrect password. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    MessageBox.Show("Database connection error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
                ' Continue the loop to retry
            Finally
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
            End Try
        Loop ' Repeat the process until successful connection

        ' Reset flag once connection process is complete
        IsDatabaseInitializing = False
    End Sub

    ' Prevent the form from closing when the user tries to close it during database initialization
    Public Sub PreventFormClosing(sender As Object, e As FormClosingEventArgs)
        If IsDatabaseInitializing Then
            ' Cancel the close action if the database is still being initialized
            e.Cancel = True
            MessageBox.Show("This form cannot be closed while the database is being initialized. Please wait.", "Action Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
End Module
