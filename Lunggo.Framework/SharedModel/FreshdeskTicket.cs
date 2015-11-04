using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Lunggo.Framework.SharedModel
{
    public class FreshdeskTicketJson
    {
        [Category("ReadOnly")]
        [Description("Ticket ID specific to your account")]
        public long display_id { get; set; }

        [Category("Mandatory")]
        [Description("Email address of the requester. If no contact exists with this email address in Freshdesk, it will be added as a new contact")]
        public string email { get; set; }
        
        [Description("User-id of the requester. For existing contacts, requester_id can be passed instead of email")]
        public long requester_id { get; set; }
        
        [Description("Ticket subject")]
        public string subject { get; set; }
        
        [Description("Plain text content of the ticket")]
        public string description { get; set; }

        [Category("Mandatory")]
        [Description("HTML content of the ticket. Description and description_html should not be passed together")]
        public string description_html { get; set; }

        [Description("Status of the ticket. (open = 2, pending  = 3, resolved = 4, closed = 5)")]
        public int status { get; set; }

        [Description("Priority of the ticket. (low = 1, medium = 2, high = 3, urgent = 4)")]
        public int priority { get; set; }

        [Description("The channel through which the ticket was created. (email = 1, portal = 2, phone = 3, forum = 4, twitter = 5, facebook = 6, chat = 7)")]
        public int source { get; set; }
        
        [Description("Set as true if the ticket is deleted/trashed. Deleted tickets will not be considered in any views except 'deleted' filter")]
        public bool deleted { get; set; }

        [Description("Set as true if the ticket is marked as spam")]
        public bool spam { get; set; }

        [Description("ID of the agent to whom the ticket is assigned")]
        public long responder_id { get; set; }

        [Description("Id of Group to which the ticket is assigned")]
        public long group_id { get; set; }

        [Description("Type property field as defined in ticket fields")]
        public string ticket_type { get; set; }

        [Description("Email address to which the incoming ticket email was sent")]
        public List<string> to_email { get; set; }

        [Description("Email address added in the 'cc' field of the incoming ticket email")]
        public List<string> cc_email { get; set; }
        
        [Description("Id of email config which is used for this ticket (i.e., support@yourcompany.com/sales@yourcompany.com)")]
        public long email_config_id { get; set; }

        [Description("Set to true if an escalation was sent")]
        public bool isescalated { get; set; }

        [Description("Ticket due-by time")]
        public DateTime due_by { get; set; }

        [Category("ReadOnly")]
        [Description("unique ID of the ticket")]
        public long id { get; set;}

        [Category("ReadOnly")]
        [Description("Attached files of the ticket")]
        public List<object> attachments { get; set; }
    }

    public class helpdesk_note
    {
        [Category("ReadOnly")]
        [Description("Id of the note")]
        public long Id { get; set; }

        [Description("Content of the note in plain text")]
        public string body { get; set; }

        [Category("Mandatory")]
        [Description("Content of the note in HTML format. Either body or body_html has to be passed")]
        public string body_html { get; set; }

        [Category("ReadOnly")]
        [Description("Attached files of the ticket")]
        public List<object> attachments { get; set; }

        [Description("user_id of the agent who is adding the note")]
        public long user_id { get; set; }

        [Description("Set as true if the note is private")]
        public bool @private { get; set; }

        [Description("Array of agent email addresses, who need to be notified")]
        public List<string> to_emails { get; set; }

        [Description("Set to true if a particular note is deleted")]
        public bool deleted { get; set; }
    }
}
