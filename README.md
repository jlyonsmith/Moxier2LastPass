## MoxierWallet to LastPass Converter

This is a simple command line tool which converts CSV data exported from the discontinued [MoxierWallet](http://www.moxier.com/wallet/) product into the CSV format required for importing into [LastPass](https://lastpass.com/).

Run the program from the command line like so:

    mono Moxier2LastPass from.csv to.csv

The program is written using [Xamarin Studio](https://xamarin.com/studio) and [Mono](http://www.mono-project.com/Main_Page).  I highly recommend you install one of the [Mono Distributions](http://www.go-mono.com/mono-downloads/download.html) if you have not done so already. 

You can download a build of the product from:

[Moxier2LastPass_1.0.10430.0.tar.gz](https://s3-us-west-2.amazonaws.com/jlyonsmith/Moxier2LastPass_1.0.10430.0.tar.gz)

The program uses the information provided by LastPass on the [Importing Passwords](https://helpdesk.lastpass.com/getting-started/importing-from-other-password-managers/) page.

MoxierWallet's CSV format is not documented anywhere than I know of.  I have converted the following types of MoxierWallet records:

- Web Logins
- Social Security
- Bank Accounts
- Credit Cards
- Insurance
- Driver License
- Notes

Your best bet is to move all your data into one of those types of record.  Alternatively, you can roll your sleeves up and add additional converters.

Enjoy!

John Lyon-Smith, April 2014. 





