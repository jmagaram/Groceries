# Groceries

I'm experimenting with Blazor for web development. This is an app to track a
grocery shopping list.

## Tech stack

There are two versions of this app in the repository. The first one I developed used Blazor
Server and SignalR. Once that seemed to work pretty well I ported it to Web Assembly.

- Uses a homemade Elm-like architecture, with most state stored in a single variable. 
- F# is used for the core data and view models
- C# Blazor is used for the front-end and server
- Azure Cosmos DB for storage
- Azure Web App
  - For the WASM app it serves some data synchronization APIs and the app itself
  - For Blazor Server, it hosts the whole thing
- Azure B2C for managing user accounts and passwords
- Straight CSS (haven't tried SCSS yet)
- Visual Studio Community

## Status/Quality

This is usable in 
- The Blazor Server version is usable but hardwired to one shopping list and has no security.
- The WASM version supports social-login and multiple families/lists. Works pretty well. 

## Hassles

There is no way to navigate from C# to F# code definitions. Features like "Find
All References" and "Go to Definition" do not work. Refactoring names in F# will
not refactor the C# references, which causes builds to break. Working with the code
requires a lot of jumping around from project to project and language to language,
and it is difficult to get into a flow.

I don't think F# breakpoints get hit when debugging in WASM. They did work in Blazor server however.

There is no hot-reload which makes development of UI slow and tedious. Build times are pretty slow too.

Javascript interop in Blazor works but is a hassle whenever I need to do it.

CSS gets unwieldy; probably need to try SCSS.

VS Professional feels a bit slow and clunky compared to VS Code.

## Pleasures

F# is awesome for the core data and view models. Love discriminated unions.
Can't do this yet in C# though the new C# records feature is a big step forward.

Blazor and C# and all the .net for front-end is amazing. Blazor server with
SignalR is like magic. Really easy debugging.

Enjoy debugging with the local Cosmos DB emulator.

CSS is much more powerful/flexible than I expected, especially animation.

Web Components are amazing. For example, I simply wrapped a list of things in a
web component and they automatically animate into place when items are moved and
inserted.

## Insights

Designing an HTML-based UI that works really well and feels good on iPhone is very difficult.

There is no room for error. Little UI glitches and inconveniences make the app almost
unusable. Animation is essential. Unlike a native app, it is difficult to work
around/with the iPhone keyboard. For example, it is difficult to make an "Add"
button that pops open a form, puts the focus in an input box, and automatically
opens the keyboard.