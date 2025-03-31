Imports MySql.Data.MySqlClient

Public Class ENROLLMENT_STUDENT

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        OBJ.Show()
        Me.Hide()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Application.ExitThread()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' Ensure that all fields are filled before inserting
        If String.IsNullOrWhiteSpace(txtregnum.Text) OrElse String.IsNullOrWhiteSpace(cmbtrack.Text) OrElse String.IsNullOrWhiteSpace(cmbsection.Text) OrElse
           String.IsNullOrWhiteSpace(cmbstatus.Text) OrElse String.IsNullOrWhiteSpace(txtname.Text) OrElse String.IsNullOrWhiteSpace(txtlrn.Text) Then
            MessageBox.Show("Please fill in all required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return ' Exit the method or prevent further execution
        End If

        ' Handle the session (afternoon or morning)
        Dim session As String = If(RadioButtonafternoon.Checked, "Afternoon", If(RadioButtonmorning.Checked, "Morning", "No session selected"))

        ' Handle the strand checkboxes (1st Year, 2nd Year, etc.)
        Dim yearLevel As String = If(CheckBox1st.Checked, "1st Year", If(CheckBox2nd.Checked, "2nd Year", "No year level selected"))

        ' Handle the strands (ABM, HUMMS, ICT, HE)
        Dim strands As String = String.Join(", ", New String() {
            If(CheckBoxabm.Checked, "ABM", ""),
            If(CheckBoxhumms.Checked, "HUMMS", ""),
            If(CheckBoxict.Checked, "ICT", ""),
            If(CheckBoxhe.Checked, "HE", "")
        }.Where(Function(str) Not String.IsNullOrEmpty(str)))

        ' If no strand is selected, set to "No strand selected"
        If String.IsNullOrEmpty(strands) Then
            strands = "No strand selected"
        End If

        ' Handle semester (1st or 2nd semester)
        Dim semester As String = If(CheckBox1st.Checked, "1st Semester", If(CheckBox2nd.Checked, "2nd Semester", "No semester selected"))

        ' Prepare the SQL query to insert data into the database
        Dim sql As String = "INSERT INTO students (`REGISTRATION_NUMBER`, `TRACK`, `SECTION`, `STATUS`, `SESSION`, `NAME`, `LRN`, `SEMESTER`, `STRANDS`) " &
                            "VALUES (@regNum, @track, @section, @status, @session, @name, @lrn, @semester, @strands)"

        ' Set up the MySQL command object
        Dim cmd As New MySqlCommand(sql, con)

        ' Add parameters to avoid SQL injection
        cmd.Parameters.AddWithValue("@regNum", txtregnum.Text)
        cmd.Parameters.AddWithValue("@track", cmbtrack.Text)
        cmd.Parameters.AddWithValue("@section", cmbsection.Text)
        cmd.Parameters.AddWithValue("@status", cmbstatus.Text)
        cmd.Parameters.AddWithValue("@session", session)
        cmd.Parameters.AddWithValue("@name", txtname.Text)
        cmd.Parameters.AddWithValue("@lrn", txtlrn.Text)
        cmd.Parameters.AddWithValue("@semester", semester)
        cmd.Parameters.AddWithValue("@strands", strands)

        Try
            ' Open the connection
            con.Open()

        Catch ex As MySqlException
            ' Handle connection-specific errors such as incorrect credentials or unreachable database
            MessageBox.Show("Connection error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return ' Exit the method early as connection failed

        End Try

        ' If connection is successful, proceed to execute the query
        Try
            ' Execute the query
            cmd.ExecuteNonQuery()

            ' Success message
            MessageBox.Show("Successfully added record!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Show OBJ1 form after successfully adding the record
            OBJ.Show()
            Me.Hide()

            ' Clear all fields after saving the record
            ClearFields()

        Catch ex As MySqlException
            ' Handle SQL execution errors (e.g., syntax errors, constraint violations)
            MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Catch ex As Exception
            ' Handle other unexpected errors
            MessageBox.Show("An unexpected error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            ' Ensure the connection is always closed, even if an error occurs
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        End Try
    End Sub

    ' Method to clear all input fields after saving the record
    Private Sub ClearFields()
        txtregnum.Clear()
        txtname.Clear()
        txtlrn.Clear()
        cmbtrack.SelectedIndex = -1 ' Clear combo box selection
        cmbsection.SelectedIndex = -1 ' Clear combo box selection
        cmbstatus.SelectedIndex = -1 ' Clear combo box selection
        RadioButtonafternoon.Checked = False
        RadioButtonmorning.Checked = False
        CheckBox1st.Checked = False
        CheckBox2nd.Checked = False
        CheckBoxabm.Checked = False
        CheckBoxhumms.Checked = False
        CheckBoxict.Checked = False
        CheckBoxhe.Checked = False
    End Sub

    ' Event handler to ensure only one checkbox for semester is checked
    Private Sub CheckBoxSemester_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1st.CheckedChanged, CheckBox2nd.CheckedChanged
        ' Ensure that when one checkbox is checked, the other is unchecked
        If sender Is CheckBox1st AndAlso CheckBox1st.Checked Then
            CheckBox2nd.Checked = False
        ElseIf sender Is CheckBox2nd AndAlso CheckBox2nd.Checked Then
            CheckBox1st.Checked = False
        End If
    End Sub

    ' Event handler to ensure only one checkbox for year level is checked
    Private Sub CheckBoxYear_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1st.CheckedChanged, CheckBox2nd.CheckedChanged
        If CheckBox1st.Checked Then
            CheckBox2nd.Checked = False
        ElseIf CheckBox2nd.Checked Then
            CheckBox1st.Checked = False
        End If
    End Sub

    ' Event handler to ensure only one checkbox for strands is checked
    Private Sub CheckBoxStrand_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxabm.CheckedChanged, CheckBoxhumms.CheckedChanged, CheckBoxict.CheckedChanged, CheckBoxhe.CheckedChanged
        ' Uncheck the others when one is selected
        If sender Is CheckBoxabm AndAlso CheckBoxabm.Checked Then
            CheckBoxhumms.Checked = False
            CheckBoxict.Checked = False
            CheckBoxhe.Checked = False
        ElseIf sender Is CheckBoxhumms AndAlso CheckBoxhumms.Checked Then
            CheckBoxabm.Checked = False
            CheckBoxict.Checked = False
            CheckBoxhe.Checked = False
        ElseIf sender Is CheckBoxict AndAlso CheckBoxict.Checked Then
            CheckBoxabm.Checked = False
            CheckBoxhumms.Checked = False
            CheckBoxhe.Checked = False
        ElseIf sender Is CheckBoxhe AndAlso CheckBoxhe.Checked Then
            CheckBoxabm.Checked = False
            CheckBoxhumms.Checked = False
            CheckBoxict.Checked = False
        End If
    End Sub

    Private Sub ENROLLMENT_STUDENT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeConnection()
    End Sub
End Class
