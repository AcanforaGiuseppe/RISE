using RISE.Models;
using RISE.Data;
using System.Collections.Generic;
using System.Linq;

namespace RISE.Data
{
    public static class DbInitializer
    {
        public static void Seed(ApplicationDbContext context)
        {
            if(!context.FAQs.Any())
            {
                var faqs = new List<FAQ>
                {
                    new FAQ
                    {
                        Question = "What is RISE Calisthenics?",
                        Answer = "RISE is a beginner to intermediate calisthenics competition focused on personal growth."
                    },
                    new FAQ
                    {
                        Question = "How can I participate in a competition?",
                        Answer = "You can register through our online form available in the Competitions section."
                    },
                    new FAQ
                    {
                        Question = "Are there categories based on weight?",
                        Answer = "Yes, we have several weight categories for fair competition."
                    },
                };

                context.FAQs.AddRange(faqs);
                context.SaveChanges();
            }
        }

    }
}