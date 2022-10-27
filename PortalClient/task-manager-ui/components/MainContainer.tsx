
import Head from "next/head"
import A from "./A"

const MainContainer = ({children}) => {
    return(
        <>
                <Head>
                  <title>Task Manager Next App</title>
                  <meta name="description" content="Task Manager Next App" />
                  <meta name="viewport" content="width=device-width, initial-scale=1" />
                  <link rel="icon" href="/favicon.ico" />
                </Head>
                  <div className="navBar">
                      <A href="/tasks" text="Tasks" />
                      <A href="/users" text="Users" />
                  </div>
                  <div>
                      {children}
                  </div>
        </>
    )
}

export default MainContainer