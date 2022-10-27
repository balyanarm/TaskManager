import * as React from 'react';
import Link from 'next/link'
import { Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from '@material-ui/core';
import  MainContainer  from '../components/MainContainer';

const Tasks = ({tasks}) => {

    return(
        <>
        <MainContainer>
        <div className='tableDiv'>   
            <h1>Task List</h1>
            <TableContainer component={Paper}>
                <Table aria-label="simple table" className="table table-striped">
                    <TableHead>
                    <TableRow>
                        <TableCell  align="right">Id</TableCell>
                        <TableCell align="right">Name</TableCell>
                    </TableRow>
                    </TableHead>
                    <TableBody>
                    {tasks.map((task: { name: string | number | boolean | React.ReactElement<any, string | React.JSXElementConstructor<any>> | React.ReactFragment | React.ReactPortal | null | undefined; id: string | number | boolean | React.ReactElement<any, string | React.JSXElementConstructor<any>> | React.ReactFragment | React.ReactPortal | null | undefined; }) => (
                        <TableRow key={task.id}>
                        <TableCell  align="right">{task.id}</TableCell>
                        <TableCell component="th" scope="row"  align="right">
                            <Link href={`/tasks/${task.id}`}>
                              <a>{task.name}</a>
                            </Link> 
                        </TableCell>
                        </TableRow>
                    ))}
                    </TableBody>
                </Table>
            </TableContainer>
        </div>
        </MainContainer>
</>
        

        
    );
};

export default Tasks
export async function getStaticProps(){
    //const response =await fetch("https://localhost:7179/api/Task/getAll")
    const response =await fetch("https://jsonplaceholder.typicode.com/users")
    
    const tasks = await response.json()
       return {
        props: {tasks},
       }
}