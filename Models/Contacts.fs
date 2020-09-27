    namespace Contacts

    module AddressBookTypes =

        type Person = { FirstName: string; LastName: string }

    module AddressBookFunctions =

        open AddressBookTypes

        let fullName (p: Person) = p.FirstName + " " + p.LastName

        type Person with
            member public me.FullName = me |> fullName

        // This works as expected
        let useFullName1 (p: Person) = p.FullName

    module SomeOtherModule =

        open AddressBookTypes
        open AddressBookFunctions

        // This will not compile
        let useFullName2 (p: Person) = p.FullName
